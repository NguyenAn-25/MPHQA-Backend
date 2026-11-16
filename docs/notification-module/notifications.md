# Quản lý thông báo

`NotificationModule` cung cấp API quản trị thông báo đã gửi. Các route yêu cầu role `ADMIN` cùng permission tương ứng trong function `NOTIFICATIONS`.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/notifications?search=...&sendTo=...` | `notifications:read` | Lấy danh sách thông báo gồm nội dung, nhóm nhận, số người nhận, số đã đọc và người tạo |
| `GET /api/v1/notifications/{id}?page=1&pageSize=20` | `notifications:read` | Xem nội dung và danh sách người nhận có phân trang |
| `POST /api/v1/notifications` | `notifications:create` | Tạo thông báo và sinh danh sách người nhận |

Danh sách mặc định trả toàn bộ kết quả, sắp theo `id` tăng dần. Truyền `page` hoặc `pageSize` mới bật phân trang; nếu thiếu một tham số thì mặc định `page = 1`, `pageSize = 20`, tối đa 100 dòng/trang. `search` tìm theo tiêu đề hoặc nội dung. `sendTo` là bộ lọc tùy chọn, nhận mã enum tiếng Anh hoặc nhãn tiếng Việt tương ứng.

Summary trả `id`, `title`, `content`, `sendTo`, `totalRecipients`, `readCount`, `createdAt` và `creator` (`id`, `publicId`, `fullname`). `readCount` là số người nhận đã đọc thông báo. Nếu creator chưa có profile, `fullname` trả `Quản trị viên`. Detail trả thông tin creator (`id`, `fullname`) và `recipients`; mỗi recipient gồm ID của notification detail, `readAt` và thông tin user (`id`, `publicId`, `phone`, `fullname`, `avatar`). Nếu user chưa có profile thì `fullname` trả chuỗi rỗng, còn `avatar` là `null`.

Với API xem chi tiết, `page` và `pageSize` chỉ áp dụng cho danh sách `recipients`. Không truyền hai tham số thì trả toàn bộ người nhận. Muốn phân trang, truyền `pageSize` (từ 1 đến 100); `page` là tùy chọn và mặc định bằng 1. Nếu truyền `page` thì phải truyền cả `pageSize`. `recipients.totalCount` và `recipients.readCount` luôn là tổng của toàn bộ người nhận, không chỉ trang hiện tại. Response khi tạo notification cũng trả toàn bộ recipients.

Response danh sách:

```json
{
  "success": true,
  "message": null,
  "data": {
    "items": [
      {
        "id": 12,
        "title": "Cập nhật lịch làm việc",
        "content": "Lịch nhận việc tuần này đã được cập nhật.",
        "sendTo": "Người dùng đã xác thực",
        "totalRecipients": 35,
        "readCount": 18,
        "createdAt": "2026-11-16T08:00:00Z",
        "creator": {
          "id": 1,
          "publicId": "00000000-0000-0000-0000-000000000001",
          "fullname": "Quản trị viên"
        }
      }
    ],
    "page": 1,
    "pageSize": 1,
    "totalCount": 1
  },
  "errors": null,
  "code": null,
  "traceId": "string"
}
```

## Tạo và gửi thông báo

`sendTo` là enum. JSON body và query nhận mã tiếng Anh hoặc nhãn tiếng Việt; response trả nhãn tiếng Việt:

| Mã tiếng Anh | Nhãn tiếng Việt trong request/response | Người nhận | Trường bổ sung |
|---|---|---|---|
| `ALL` | `Tất cả người dùng` | Tất cả tài khoản role `USER` | Không gửi `userId` |
| `VERIFIED_USERS` | `Người dùng đã xác thực` | Tài khoản role `USER` đã xác thực danh tính (`identity_verified_at` có giá trị) | Không gửi `userId` |
| `SPECIFIC_USER` | `Người dùng cụ thể` | Một tài khoản role `USER` | Bắt buộc `userId` |

Ví dụ gửi cho nhóm người dùng đã xác thực:

```json
{
  "title": "Cập nhật lịch làm việc",
  "content": "Lịch nhận việc tuần này đã được cập nhật.",
  "sendTo": "Người dùng đã xác thực"
}
```

Ví dụ gửi cho một người:

```json
{
  "title": "Nhắc bổ sung hồ sơ",
  "content": "Vui lòng cập nhật thông tin hồ sơ của bạn.",
  "sendTo": "Người dùng cụ thể",
  "userId": 24
}
```

Khi gửi theo nhóm không có người nhận phù hợp, API trả lỗi thay vì tạo thông báo rỗng. Tạo thành công trả HTTP `201 Created` cùng detail và danh sách notification detail vừa được tạo; tất cả người nhận mới có `readAt = null`.

Profile `ADMIN_DEFAULT` có `notifications:read` và `notifications:create`. Migration `AddNotificationFunctionCatalog` thêm function/grant này vào database đã có; seeder đồng bộ catalog trong Development. EF converter lưu `send_to` dưới dạng mã tương thích `ALL_USERS`, `VERIFIED_USERS` hoặc `USER` để đọc và lọc được dữ liệu hiện có.

Trong môi trường Development, `NotificationDevelopmentSampleDataSeeder` tạo 30 thông báo mẫu cùng notification detail cho các nhóm `ALL`, `VERIFIED_USERS` và `SPECIFIC_USER`. Dữ liệu có cả người nhận đã đọc và chưa đọc; seeder có thể chạy lại mà không tạo trùng thông báo hoặc recipient.

Ví dụ response chi tiết notification:

```json
{
  "success": true,
  "message": null,
  "data": {
    "id": 12,
    "title": "Cập nhật lịch làm việc",
    "content": "Lịch nhận việc tuần này đã được cập nhật.",
    "sendTo": "Người dùng đã xác thực",
    "createdAt": "2026-11-16T08:00:00Z",
    "creator": {
      "id": 1,
      "fullname": "Quản trị viên"
    },
    "recipients": {
      "items": [
        {
          "id": 501,
          "readAt": null,
          "user": {
            "id": 24,
            "publicId": "00000000-0000-0000-0000-000000000024",
            "phone": "+12025550123",
            "fullname": "Nguyễn Văn A",
            "avatar": null
          }
        }
      ],
      "page": 1,
      "pageSize": 20,
      "totalCount": 35,
      "readCount": 18
    }
  },
  "errors": null,
  "code": null,
  "traceId": "string"
}
```
