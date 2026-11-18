# Cấu hình AuthModule

Cấu hình chung .NET/PostgreSQL, chạy ứng dụng và migration nằm trong [Cấu hình và chạy hệ thống](../system-configuration.md). Trang này tập trung vào JWT và OTP.

## JWT

AuthModule bind section `Jwt` vào `JwtOptions`, validate khi ứng dụng khởi động:

| Key | Mặc định | Giới hạn |
|---|---:|---:|
| `Jwt:Issuer` | `DotNetSkeleton` | Bắt buộc |
| `Jwt:Audience` | `DotNetSkeleton.Client` | Bắt buộc |
| `Jwt:SigningKey` | Không có | Bắt buộc, tối thiểu 32 ký tự |
| `Jwt:AccessTokenMinutes` | 15 phút | 1–60 phút |
| `Jwt:RefreshTokenDays` | 30 ngày | 1–90 ngày |
| `Jwt:OtpMinutes` | 5 phút | 1–15 phút |

Tạo key ngẫu nhiên và lưu vào User Secrets khi phát triển local:

```bash
dotnet user-secrets set "Jwt:SigningKey" "$(openssl rand -base64 48)"
```

Production đặt key trong secret manager/environment; không commit key hoặc đưa vào `appsettings.json`.

## Gửi OTP

Ứng dụng hiện không tích hợp nhà cung cấp SMS. `LoggingOtpSmsSender` ghi OTP vào application log ở mọi môi trường để luồng đăng ký và đặt lại mật khẩu có thể thử mà không cần cấu hình dịch vụ ngoài. Development dùng OTP `012345`; môi trường khác sinh mã ngẫu nhiên 6 chữ số.

Không dùng sender ghi log cho người dùng thật vì mã OTP sẽ xuất hiện trong log. Trước khi triển khai công khai, cần tích hợp một nhà cung cấp gửi OTP phù hợp.

## Các cấu hình Production của Auth

```bash
export Jwt__Issuer="DotNetSkeleton"
export Jwt__Audience="DotNetSkeleton.Client"
export Jwt__SigningKey="replace-with-a-random-secret-at-least-32-characters"
export Jwt__AccessTokenMinutes="15"
export Jwt__RefreshTokenDays="30"
export Jwt__OtpMinutes="5"
```

Giá trị secret phải được cung cấp qua secret manager của môi trường triển khai; ví dụ trên minh họa tên key và cấu trúc, không phải chỗ lưu secret lâu dài.

## Tài khoản admin local trong Development

Khi chạy Development, `AuthDevelopmentAdminAccountsSeeder` tự tạo hai tài khoản ADMIN giả lập:

| Số điện thoại | Mật khẩu |
|---|---|
| `+12025550190` | `123456` |
| `+12025550191` | `123456` |

Đăng nhập bằng số điện thoại và mật khẩu ở bảng trên. Đây là tài khoản fixture với số điện thoại giả lập, chỉ dùng local Development; Production không chạy seeder này.

Để thử các endpoint quản trị user ở máy local, có thể cấu hình thêm một admin Development:

```bash
dotnet user-secrets set "Seed:AdminPhone" "+84901234567"
dotnet user-secrets set "Seed:AdminPassword" "ReplaceMe@2026"
```

`AuthDevelopmentAdminSeeder` tạo user active có role `ADMIN` và permission profile `ADMIN_DEFAULT`, hash mật khẩu bằng `PasswordHasher` và không tạo session/OTP. Mật khẩu cấu hình phải dài 8–128 ký tự, có chữ in hoa, chữ số và ký tự đặc biệt. Luồng này bỏ qua xác minh số điện thoại, vì vậy chỉ dùng trong Development. Seeder không sửa user đã tồn tại và sẽ báo lỗi nếu số điện thoại cấu hình thuộc một tài khoản khác. Xóa hai User Secrets này nếu không muốn seeder tạo tài khoản tùy chỉnh.

`AuthDevelopmentSampleDataSeeder` tạo tối đa 60 user giả lập với số điện thoại `+12025550100`–`+12025550159`; password là `123456`, OTP/session mẫu đều hết hạn. Trên database mới có 36 user active, 8 user chờ xác minh và 16 user đã xác minh nhưng bị tắt. `ProfileDevelopmentSampleDataSeeder` thêm hồ sơ, kinh nghiệm, kỹ năng, yêu cầu xác thực và identity mẫu liên kết với các user này. Các seeder chỉ chạy trong Development và không ghi đè profile/request đã tồn tại.

## Rate limit Auth

Các route Auth hiện áp dụng sliding window theo IP, tối đa 10 request trong một phút. Request vượt ngưỡng nhận HTTP 429 với code `rate_limit_exceeded`. Cấu hình này nằm trong `Extensions/AuthRateLimitingServiceCollectionExtensions`; đây là giới hạn cho controller Auth hiện tại.

## Hành vi token

- Access token mặc định sống 15 phút.
- Refresh token chỉ được lưu dạng SHA-256 hash; thời hạn session mặc định 30 ngày.
- OTP mặc định có hiệu lực 5 phút và được lưu dạng PasswordHasher hash.
- Trong Development, OTP là `012345`; ngoài Development, OTP là chuỗi số ngẫu nhiên 6 chữ số. OTP được lưu dạng hash và có purpose riêng. Mỗi OTP có tối đa 5 lần nhập sai; yêu cầu gửi lại cùng purpose cách nhau ít nhất 30 giây.

Chi tiết luồng nằm trong [Luồng xác thực](authentication.md); route và quyền quản trị nằm trong [User và permission](users-and-permissions.md).
