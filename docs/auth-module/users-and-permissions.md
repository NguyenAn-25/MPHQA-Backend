# Quản lý người dùng và phân quyền

## API quản lý user

Các endpoint nằm dưới `/api/v1/users`. Chúng yêu cầu access token, role `ADMIN` và permission tương ứng.

Response JSON dùng envelope chung và đặt DTO bên trong `data`; tạo tài khoản trả HTTP `201 Created` cùng Location. Xem [Quy ước API](../api-conventions.md) để biết cấu trúc response.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/users?search=...` | `users:read` | Lọc toàn bộ user, mặc định trả đầy đủ summary |
| `GET /api/v1/users/{id}` | `users:read` | Xem chi tiết user theo ID |
| `POST /api/v1/users` | `users:create` | Tạo tài khoản ADMIN bằng phone, password và permission |
| `PUT /api/v1/users/{id}` | `users:update` | Đổi permission profile của ADMIN |
| `PUT /api/v1/users/{id}/password` | `users:update` | Đổi password của user và thu hồi session hiện có |
| `PATCH /api/v1/users/{id}/status` | `users:update` | Bật hoặc tắt user |

Các endpoint là công cụ quản trị, yêu cầu access token hợp lệ, role `ADMIN` và permission tương ứng. Danh sách và xem chi tiết bao gồm cả tài khoản `USER` đăng ký công khai lẫn tài khoản `ADMIN`. Tạo mới qua API quản trị luôn tạo `ADMIN`. Nếu không truyền `page` và `pageSize`, danh sách trả đầy đủ kết quả sau lọc. Truyền ít nhất một trong hai tham số sẽ bật phân trang; nếu chỉ truyền `page`, `pageSize` mặc định là 20; nếu chỉ truyền `pageSize`, `page` mặc định là 1. `pageSize` tối đa 100. API trả `PagedUsersResponseDto` gồm summary `items`, `page`, `pageSize` và `totalCount`.

Danh sách user được sắp xếp theo `id` tăng dần.

### Tiêu chí lọc danh sách

Có thể kết hợp các query parameter sau:

| Parameter | Ý nghĩa |
|---|---|
| `search` | Tìm chuỗi con trong số điện thoại |
| `role` | Lọc theo role; nhận `USER`/`ADMIN` hoặc `Người dùng`/`Quản trị viên` |
| `isActive` | Lọc trạng thái bật/tắt (`true`/`false`) |
| `isVerified` | Lọc trạng thái xác minh (`true`/`false`) |
| `permissionId` | Lọc user được gán permission profile này |
| `createdFrom` | Lọc từ thời điểm tạo trở đi, ISO 8601, inclusive |
| `createdTo` | Lọc đến thời điểm tạo, ISO 8601, inclusive |
| `page`, `pageSize` | Không truyền thì lấy toàn bộ; truyền một tham số thì bật phân trang, mặc định phần còn thiếu là 1/20; tối đa 100 dòng/trang |

Ví dụ: `GET /api/v1/users?role=USER&isActive=true&isVerified=true&search=1202&page=1&pageSize=20`. Giá trị tiếng Việt của `role` cần được URL encode trong request thực tế.

### Tạo user

Admin tạo tài khoản quản trị trực tiếp bằng số điện thoại, mật khẩu ban đầu và permission profile đang hoạt động. Role luôn là `ADMIN`, nên request không nhận `role`. Số điện thoại phải theo E.164 và chưa được tài khoản nào sử dụng (kể cả tài khoản `USER` đăng ký công khai). Mật khẩu dài 8–128 ký tự, có chữ in hoa, chữ số và ký tự đặc biệt. Tài khoản được tạo active và verified; luồng tạo bởi admin không dùng OTP.

```json
{
  "phone": "+84901234567",
  "password": "StrongPass@123",
  "permissionId": 2
}
```

Số điện thoại được kiểm tra trước khi tạo và unique constraint của database cũng được xử lý để tránh trùng khi có hai request đồng thời. Tạo thành công trả `201 Created` cùng detail DTO và `Location` trỏ đến tài khoản vừa tạo.

### Cập nhật user

`PUT /api/v1/users/{id}` chỉ thay đổi permission profile. `{id}` là ID nội bộ dạng số. Chỉ có thể chọn profile đang active:

```json
{
  "permissionId": 2
}
```

Phone và role không được cập nhật qua endpoint này. Chỉ tài khoản role `ADMIN` có permission profile; gửi yêu cầu đổi permission cho tài khoản `USER` sẽ nhận lỗi 400. Khi permission thay đổi, các session đang hoạt động bị revoke để người dùng đăng nhập lại và nhận claim mới.

### Đổi password

`PUT /api/v1/users/{id}/password` nhận password mới theo cùng quy tắc mạnh như đăng ký. Hệ thống lưu password hash, không trả password trong response và thu hồi mọi session hiện có. Thành công trả `204 No Content`.

```json
{
  "newPassword": "NewStrongPass@456"
}
```

### Bật hoặc tắt tài khoản

`PATCH /api/v1/users/{id}/status` nhận:

```json
{
  "isActive": false
}
```

Tắt user sẽ revoke các session chưa revoke. Chỉ bật lại tài khoản đã verified; tài khoản chờ xác minh phải hoàn tất đăng ký trước. Bật lại user không tự khôi phục session cũ; người dùng đăng nhập lại để tạo session mới.

## Mô hình permission

Ba khái niệm được ghép lại như sau:

- `functions` khai báo mã chức năng ổn định, tên hiển thị tiếng Việt/tiếng Anh và các `actions` được hỗ trợ.
- `user.role` là role của tài khoản, chỉ nhận `USER` hoặc `ADMIN`.
- `permissions` là các permission profile chứa tập function/action; chỉ user `ADMIN` mới được gắn profile qua `user.permission_id`.
- `permission_details` là danh sách action được cấp cho từng permission profile.

`USER` phải có `permission_id = NULL`; `ADMIN` phải tham chiếu một permission profile đang hoạt động. Database có check constraint và service cũng kiểm tra quy tắc này. Một permission profile có thể gắn cho nhiều admin; chỉnh `permission_details` sẽ cập nhật quyền của tất cả admin đang dùng profile đó. Profile mặc định là `ADMIN_DEFAULT`; có thể tạo thêm profile với các grant khác nhau.

Policy được viết theo dạng:

```csharp
[Authorize(Policy = "permission:users:read")]
```

Policy provider tách `users` thành function và `read` thành action. JWT chứa role claim `ADMIN`/`USER`; claim `permission` chỉ được thêm cho user `ADMIN` có permission profile đang hoạt động. User `USER` không nhận claim phân quyền động. Authorization handler so khớp claim, ví dụ `users:read`.

Claim được chụp vào JWT lúc tạo session. Khi thay đổi trực tiếp grant ngoài API, cần revoke session của admin để token mới phản ánh quyền hiện tại.

## API quản lý nhóm quyền

API danh sách function và API quản lý nhóm quyền dùng hai route riêng. Danh sách function chỉ yêu cầu role `ADMIN`; API quản lý nhóm quyền yêu cầu role `ADMIN` cùng permission tương ứng trong function `permissions`. Function trả `id`, `code`, `nameVi`, `nameEn` và `actions`; code và action dùng chữ in hoa:

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/functions` | Role `ADMIN` | Lấy catalog function tiếng Việt/Anh cùng các action khả dụng |
| `GET /api/v1/permissions` | `permissions:read` | Lấy summary nhóm quyền kèm số tài khoản đang sử dụng |
| `GET /api/v1/permissions/{id}` | `permissions:read` | Xem chi tiết nhóm quyền và toàn bộ function/action |
| `POST /api/v1/permissions` | `permissions:create` | Tạo nhóm quyền cùng danh sách grant |
| `PUT /api/v1/permissions/{id}` | `permissions:update` | Cập nhật tên, mô tả, `isActive` và thay toàn bộ danh sách grant |
| `PATCH /api/v1/permissions/{id}/status` | `permissions:update` | Bật hoặc ẩn nhóm quyền qua `isActive` |
| `DELETE /api/v1/permissions/{id}` | `permissions:delete` | Xóa nhóm quyền nếu không có tài khoản sử dụng |

Danh sách nhóm quyền mặc định trả toàn bộ kết quả đã lọc, sắp theo `id` tăng dần. Truyền `page` hoặc `pageSize` mới bật phân trang; `pageSize` mặc định là 20, `page` mặc định là 1 và `pageSize` tối đa 100. Có thể lọc theo `search` (tên nhóm quyền) và `isActive` (`true`/`false`). Mỗi summary có `id`, `name`, `description`, `isActive`, `userCount`; detail thêm `details`, mỗi grant gồm `function` (`id`, `code`, `nameVi`, `nameEn`) và `action`.

Request tạo nhóm quyền:

```json
{
  "name": "Điều phối viên",
  "description": "Quản lý công việc và hồ sơ ứng viên.",
  "details": [
    { "functionId": 1, "action": "READ" },
    { "functionId": 2, "action": "READ" }
  ]
}
```

`PUT /api/v1/permissions/{id}` nhận thêm `isActive` trong body, ví dụ `"isActive": true`. Để bật hoặc ẩn riêng nhóm quyền, `PATCH /api/v1/permissions/{id}/status` nhận body `{ "isActive": false }`.

`details` là bắt buộc khi tạo và cập nhật, nhưng có thể gửi `[]` để tạo nhóm trống hoặc gỡ toàn bộ quyền. `functionId` phải tồn tại và action phải nằm trong catalog trả về từ `/functions`; grant trùng lặp bị từ chối. Khi cập nhật, `isActive` là boolean bắt buộc (`true`/`false`); khi tạo nhóm mới, hệ thống mặc định đặt `isActive = true`. Có thể xóa nhóm quyền không được gán cho tài khoản; nếu đang được sử dụng API trả `409 Conflict`. Thay đổi `isActive` hoặc grant sẽ thu hồi session của admin dùng nhóm quyền để token mới phản ánh đúng quyền; sửa tên/mô tả không bắt buộc đăng nhập lại.

## Catalog mặc định

Seeder và migration khai báo đúng function/action đang được dùng bởi các endpoint quản lý user:

| Code | Name VI | Name EN | Actions |
|---|---|---|---|
| `USERS` | Quản lý người dùng | User Management | `READ`, `CREATE`, `UPDATE` |
| `JOB_CATEGORIES` | Quản lý lĩnh vực | Job Category Management | `READ`, `CREATE`, `UPDATE`, `DELETE` |
| `SKILLS` | Quản lý kỹ năng | Skill Management | `READ`, `CREATE`, `UPDATE`, `DELETE` |
| `VERIFICATION_REQUESTS` | Xử lý yêu cầu xác thực | Verification Request Management | `READ`, `UPDATE` |
| `NOTIFICATIONS` | Quản lý thông báo | Notification Management | `READ`, `CREATE` |
| `PERMISSIONS` | Quản lý nhóm quyền | Permission Management | `READ`, `CREATE`, `UPDATE`, `DELETE` |

Profile `ADMIN_DEFAULT` khởi tạo với ba grant `users:read`, `users:create`, `users:update` cùng quyền quản lý lĩnh vực, kỹ năng, yêu cầu xác thực, thông báo và nhóm quyền. Seeder chỉ thêm bộ grant ban đầu khi tạo profile này lần đầu; migration module bổ sung grant mới cho profile mặc định đã tồn tại. Những lần khởi động sau không tự khôi phục grant đã bị gỡ. Có thể tạo permission profile khác rồi cấp action phù hợp qua API nhóm quyền. Xem [API quản lý lĩnh vực](../job-module/job-categories.md), [API quản lý kỹ năng](../skill-module/skills.md), [hồ sơ cá nhân](../profile-module/profile.md), [admin xử lý yêu cầu xác thực](../profile-module/verification-requests.md) và [quản lý thông báo](../notification-module/notifications.md).

### Tạo tài khoản quản trị ban đầu

Migration và `AuthPermissionCatalogSeeder` tạo profile `ADMIN_DEFAULT`. Trong Development, `AuthDevelopmentAdminAccountsSeeder` tự tạo hai tài khoản ADMIN giả lập để thử local: `+12025550190` và `+12025550191`, cùng mật khẩu `123456` như user mẫu. Seeder không nâng quyền tài khoản có sẵn; nếu fixture admin đã được tạo với mật khẩu cũ `Admin@123456`, seeder sẽ cập nhật về `123456`, còn số điện thoại thuộc cấu hình tài khoản khác thì báo lỗi. Có thể tạo thêm một admin với số điện thoại/mật khẩu riêng bằng `Seed:AdminPhone` và `Seed:AdminPassword` trong User Secrets. Tất cả tài khoản được seed đều active, có profile `ADMIN_DEFAULT` và bỏ qua xác minh OTP; chỉ dùng thông tin fixture trong local Development, không dùng chúng ở Production.

Trong Development, `AuthDevelopmentSampleDataSeeder` tạo tối đa 60 user mẫu với số điện thoại trong dải `+12025550100`–`+12025550159` và password `123456`. Database mới có 36 tài khoản active, 8 tài khoản chờ xác minh và 16 tài khoản đã xác minh nhưng bị tắt. OTP/session mẫu đều hết hạn. `ProfileDevelopmentSampleDataSeeder` tạo hồ sơ, kinh nghiệm, kỹ năng, yêu cầu xác thực ở các trạng thái khác nhau và identity cho các yêu cầu được chấp nhận. Seeder không ghi đè dữ liệu hồ sơ hoặc yêu cầu đã tồn tại, nên dữ liệu người dùng đã có được giữ nguyên khi chạy lại.

Nếu thực hiện bootstrap trực tiếp bằng PostgreSQL, chỉ người vận hành có quyền truy cập DB mới nên chạy câu lệnh tương đương sau, thay UUID bằng `public_id` của tài khoản đã xác minh:

```sql
BEGIN;
UPDATE "user"
SET role = 'ADMIN',
    permission_id = (SELECT id FROM permissions WHERE name = 'ADMIN_DEFAULT' AND is_active = true)
WHERE public_id = '00000000-0000-0000-0000-000000000000'
  AND is_active = true
  AND is_verified = true;
UPDATE auth_sessions
SET revoked_at = now()
WHERE user_id = (
    SELECT id FROM "user"
    WHERE public_id = '00000000-0000-0000-0000-000000000000'
)
  AND revoked_at IS NULL;
COMMIT;
```

Sau khi đổi role trực tiếp trong database, cần revoke session như trong ví dụ rồi người dùng đăng nhập lại để JWT mới nhận permission claim tương ứng. Nếu bỏ qua bước revoke, JWT cũ vẫn giữ claim đã được phát trước đó.

## Response và quyền riêng tư

Summary khi xem danh sách gồm `id`, `publicId`, phone, `isActive`, `isVerified`, `createdAt`, role và object `permission` (`id`, `name`); permission là `null` với role `USER`. Detail thêm `verifiedAt`, `lastActiveAt` và object permission đầy đủ hơn (`id`, `name`, `description`, `isActive`). Permission được trả chung trong object, không tách `permissionId` và tên nhóm quyền ở cấp user. Các trường password/hash không được trả trong DTO. Role được trả bằng nhãn tiếng Việt; trạng thái permission là boolean `isActive` và PostgreSQL lưu bằng cột `is_active`.
