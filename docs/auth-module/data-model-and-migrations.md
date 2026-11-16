# Mô hình dữ liệu AuthModule

Trang này mô tả sáu entity/bảng thuộc AuthModule và catalog mặc định. Lệnh EF Core, lifecycle migration và môi trường chạy nằm trong [Cơ sở dữ liệu và migration toàn hệ thống](../persistence-and-migrations.md).

## Sáu bảng

| Bảng | Vai trò | Trường và ý nghĩa |
|---|---|---|
| `user` | Hồ sơ đăng nhập | `id` khóa nội bộ; `public_id` ID công khai; `phone` duy nhất; `password` password hash; `is_active` tài khoản đang được phép hoạt động; `is_verified` trạng thái xác minh; `verified_at` thời điểm tài khoản được xác minh; `role` (`USER`/`ADMIN`); `permission_id` chỉ dùng cho `ADMIN`; `last_active_at`; `created_at` |
| `auth_sessions` | Theo dõi phiên đăng nhập và refresh token | `user_id` chủ sở hữu; `refresh_token` SHA-256 hash; `created_at`, `expired_at` và `revoked_at` |
| `auth_otps` | Lưu OTP đăng ký/reset | `public_id` làm challenge ID cho API; `user_id`; `code` hash; `purpose`; `failed_attempts`; `created_at`, `expired_at`, `verified_at` |
| `permissions` | Permission chứa các grant cho user `ADMIN` | `is_active`, `name`, `description` |
| `permission_details` | Grant một action cho permission profile | `permission_id`, `function_id`, `action` |
| `functions` | Danh mục nhóm chức năng API và action tương ứng | `code` duy nhất; `name_vi`; `name_en`; `actions` kiểu PostgreSQL `text[]` |

## Quan hệ

```mermaid
erDiagram
    permissions ||--o{ user : grants
    user ||--o{ auth_sessions : owns
    user ||--o{ auth_otps : owns
    permissions ||--o{ permission_details : contains
    functions ||--o{ permission_details : describes
```

- `USER` luôn có `permission_id = NULL`; `ADMIN` phải tham chiếu một permission profile.
- `is_verified` được đặt thành `true` và `verified_at` được ghi cùng thời điểm khi OTP đăng ký đã xác minh và tài khoản hoàn tất đăng ký; đổi số điện thoại đặt `is_verified = false`, `verified_at = NULL`. Database ràng buộc hai trường phải đồng bộ. `is_active` độc lập để quản trị viên có thể tạm khóa/mở lại tài khoản đã xác minh.
- `user.verified_at` lưu trạng thái xác minh lâu dài của tài khoản; `auth_otps.verified_at` chỉ lưu lúc một challenge OTP cụ thể được xác minh.
- Một permission profile có thể gán cho nhiều user `ADMIN`; mỗi user tham chiếu tối đa một profile.
- Một user có thể có nhiều session và OTP. Xóa user cascade xóa session/OTP.
- `permission_details` nối một permission với một function/action.
- Xóa permission hoặc function cascade xóa các grant tương ứng.

## Khóa và ràng buộc

- `user.public_id`, `user.phone`, `auth_otps.public_id`, `permissions.name`, `functions.code` có unique index. Client dùng `auth_otps.public_id` cho bước xác minh OTP và bước hoàn tất đăng ký/đặt lại mật khẩu.
- `permission_details` unique theo tổ hợp `permission_id + function_id + action`, tránh grant lặp.
- `auth_sessions.refresh_token` có unique index; `auth_otps` có index theo `user_id`, `purpose`, `created_at` để tìm OTP gần nhất.
- Password và OTP dùng ASP.NET Core `PasswordHasher`; refresh token lưu SHA-256. Không lưu ba giá trị này dạng rõ.
- Timestamp map thành PostgreSQL `timestamp with time zone`; service tạo thời gian UTC.

## Permission catalog được nạp

Migration `AddUserVerificationState` đảm bảo có `user.is_verified` và `user.verified_at`, kể cả khi `is_verified` đã được tạo thủ công trước đó, rồi thêm check constraint giữ hai trường nhất quán. Dữ liệu cũ được backfill: user đang active được xem là đã xác minh (thời điểm lấy từ OTP đăng ký gần nhất nếu OTP đó đã được xác minh, hoặc `created_at` nếu OTP đó chưa xác minh hay chưa có); user inactive chỉ được xem là đã xác minh nếu OTP đăng ký gần nhất đã được xác minh.

Migration `LimitOtpVerificationAttempts` thêm `auth_otps.failed_attempts`; một mã OTP hết hiệu lực sau 5 lần nhập sai.

`SeparateUserRoleAndPermissions` thêm `user.role`, chuyển user đang gắn profile `ADMIN` thành role `ADMIN`, bỏ profile `USER`, đổi profile `ADMIN` thành `ADMIN_DEFAULT`, và thêm check constraint role/permission. `SetAuthEnumsUppercase` chuẩn hóa các enum trong PostgreSQL thành mã tiếng Anh in hoa (`REGISTRATION`, `PASSWORD_RESET`); trạng thái permission cũ cũng từng được lưu thành `ACTIVE`/`INACTIVE` trước khi `ReplacePermissionStatusWithIsActive` chuyển chúng sang boolean. Converter role trả nhãn tiếng Việt trong JSON. `AuthPermissionCatalogSeeder` chạy sau migration trong Development và cập nhật function catalog. Bộ grant mặc định chỉ được thêm khi profile `ADMIN_DEFAULT` được tạo lần đầu; lần chạy sau không khôi phục grant bị gỡ. `AuthDevelopmentAdminAccountsSeeder` tạo hai admin fixture, còn `AuthDevelopmentAdminSeeder` có thể tạo thêm admin local theo User Secrets; cả hai chỉ chạy trong Development.

| Code | Grant ban đầu của `ADMIN_DEFAULT` |
|---|---|
| `USERS` | `READ`, `CREATE`, `UPDATE` |
| `JOB_CATEGORIES` | `READ`, `CREATE`, `UPDATE`, `DELETE` |
| `SKILLS` | `READ`, `CREATE`, `UPDATE`, `DELETE` |
| `VERIFICATION_REQUESTS` | `READ`, `UPDATE` |
| `NOTIFICATIONS` | `READ`, `CREATE` |
| `PERMISSIONS` | `READ`, `CREATE`, `UPDATE`, `DELETE` |

Function catalog có các code `USERS`, `JOB_CATEGORIES`, `SKILLS`, `VERIFICATION_REQUESTS`, `NOTIFICATIONS` và `PERMISSIONS` với các action tương ứng policy đang gắn trên controller. `Code` là mã ổn định dùng để tạo permission claim; `NameVi`/`NameEn` là tên hiển thị, `Actions` được lưu và trả bằng chữ in hoa. Migration `UpdateFunctionCatalogFields` đổi schema và chuyển dữ liệu cũ; `RenameFunctionAvailableActionsToActions` chuẩn hóa tên cột. Migration `ReplacePermissionStatusWithIsActive` chuyển trạng thái permission hiện có thành cờ boolean `is_active`; `AddNotificationFunctionCatalog` thêm function/grant thông báo cho `ADMIN_DEFAULT`. Migration `RemoveLegacyPermissionProfilesFunction` dọn function trùng tên cũ và chuyển grant hiện có sang `PERMISSIONS`. JWT chuẩn hóa code/action thành chữ thường để khớp policy. Chi tiết xem [User và permission](users-and-permissions.md).

Migration và `AuthPermissionCatalogSeeder` tạo profile `ADMIN_DEFAULT`. Trong Development, tài khoản ADMIN fixture `+12025550190` và `+12025550191` dùng mật khẩu `123456`, giống user mẫu; có thể cấu hình thêm một admin riêng bằng `Seed:AdminPhone` và `Seed:AdminPassword`. Startup Production không chạy runtime seeder; dữ liệu catalog production phải có trong migration/release SQL và tài khoản quản trị cần được bootstrap qua quy trình kiểm soát riêng.

`AuthDevelopmentSampleDataSeeder` tạo tối đa 60 user mẫu ở Development; database mới có 36 active, 8 chờ xác minh và 16 đã xác minh nhưng bị tắt. Password mặc định là `123456`; OTP/session mẫu đều hết hạn. `ProfileDevelopmentSampleDataSeeder` bổ sung hồ sơ, kinh nghiệm thuộc danh mục việc ngắn hạn, kỹ năng, verification request ở nhiều trạng thái và identity cho request được chấp nhận. Seeder chỉ thêm dữ liệu còn thiếu và giữ nguyên hồ sơ/request đã có.

## Truy vấn kiểm tra catalog

```sql
SELECT p.name AS permission, f.name_en AS function, pd.action
FROM permission_details pd
JOIN permissions p ON p.id = pd.permission_id
JOIN functions f ON f.id = pd.function_id
ORDER BY p.name, f.code, pd.action;
```

## Thay đổi schema AuthModule

Entity nằm trong `Modules/AuthModule/Entities/`; model được tập hợp vào `AppDbContext`. Khi thay đổi entity, tạo migration mới theo [quy trình migration toàn hệ thống](../persistence-and-migrations.md). Migration `Down()` hiện tại xóa sáu bảng trên cùng dữ liệu của chúng; cần đánh giá dữ liệu trước khi rollback.
