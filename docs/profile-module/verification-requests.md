# Xử lý yêu cầu xác thực

`ProfileModule` cung cấp API gửi và xử lý yêu cầu xác thực giấy tờ cùng với các chức năng hồ sơ. API danh sách của Admin, xem chi tiết và cập nhật trạng thái yêu cầu cần role `ADMIN` cùng permission của AuthModule; profile `ADMIN_DEFAULT` được cấp `verification_requests:read` và `verification_requests:update`. API tạo yêu cầu và lấy danh sách yêu cầu của bản thân cần role `USER`.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `POST /api/v1/verification-requests` | User đăng nhập | Tạo yêu cầu `PENDING`; sender lấy từ access token. Response `201 Created` trả cùng cấu trúc chi tiết như API GET theo ID. |
| `GET /api/v1/verification-requests?status=PENDING&createdFrom=...&createdTo=...` | `verification_requests:read` | Danh sách summary đầy đủ, lọc theo trạng thái và ngày tạo yêu cầu. |
| `GET /api/v1/verification-requests/me?status=PENDING&createdFrom=...&createdTo=...` | User đăng nhập | Danh sách verification request của user hiện tại, có cùng bộ lọc và phân trang; sender lấy từ access token. |
| `GET /api/v1/verification-requests/{id}` | `verification_requests:read` | Xem giấy tờ, ảnh chân dung, người gửi, hồ sơ và danh tính hiện tại để đối chiếu. |
| `PATCH /api/v1/verification-requests/{id}/status` | `verification_requests:update` | Chấp nhận hoặc từ chối yêu cầu đang PENDING. |

`status` là enum `VerificationRequestStatus`. Query và body nhận mã `PENDING`, `ACCEPTED`, `REJECTED`, `CANCELLED` hoặc nhãn tiếng Việt tương ứng; JSON response trả nhãn tiếng Việt. EF converter vẫn lưu/đọc các mã uppercase trong cột database. Nhãn tiếng Việt trên query string cần được URL encode:

| Mã lưu và giá trị filter | Giá trị `status` trong response |
|---|---|
| `PENDING` | `Đang chờ duyệt` |
| `ACCEPTED` | `Đã chấp nhận` |
| `REJECTED` | `Đã từ chối` |
| `CANCELLED` | `Đã hủy` |

`createdFrom` và `createdTo` là ISO 8601, inclusive. `page` và `pageSize` là tùy chọn, mặc định tương ứng 1 và 10; phân trang chỉ bật khi truyền ít nhất một tham số, nếu không API trả toàn bộ kết quả. Quy tắc lọc và phân trang này áp dụng cho cả danh sách Admin và `GET /me`. Request từ chối phải có lý do:

User gửi yêu cầu bằng `POST` với họ tên trên thẻ, số ID và ba ảnh mặt trước, mặt sau, chân dung. Không nhận `senderId` từ body; người gửi lấy từ JWT. Nếu chưa có `Profile`, lúc gửi yêu cầu hệ thống tạo profile tối thiểu bằng họ tên trên thẻ. Mỗi user chỉ có tối đa một yêu cầu `PENDING`; gửi trùng khi đang chờ trả `409 Conflict`.

Không truyền cả `page` và `pageSize` thì API trả toàn bộ kết quả sau khi lọc; response có `page=1` và `pageSize` bằng số item trả về. Truyền một trong hai tham số sẽ bật phân trang; tham số còn thiếu mặc định là `page=1` hoặc `pageSize=10`.

Danh sách được sắp xếp theo `id` tăng dần.

Chi tiết yêu cầu trả `createdAt`, `verifiedAt`, `rejectedAt` ở cấp yêu cầu. `sender` gồm thông tin tài khoản, `profile` và `currentIdentity`; `profile` hoặc `currentIdentity` có thể là `null` nếu dữ liệu hồ sơ tương ứng chưa tồn tại. `sender.isVerified` và `sender.verifiedAt` là trạng thái xác thực tài khoản qua OTP; `verifiedAt` ở cấp yêu cầu là thời điểm Admin chấp nhận xác thực giấy tờ.

Khi tạo yêu cầu thành công, `POST /api/v1/verification-requests` trả `201 Created` với cùng cấu trúc response chi tiết bên dưới. Yêu cầu mới có `status` là `Đang chờ duyệt`, `verifiedAt` và `rejectedAt` là `null`; thông tin `sender` lấy từ user đang đăng nhập.

```json
{
  "id": 1,
  "fullNameOnCard": "NGUYEN VAN A",
  "idNumber": "012345678901",
  "cardFrontImage": "https://cdn.example.com/id-front.jpg",
  "cardBackImage": "https://cdn.example.com/id-back.jpg",
  "portraitImage": "https://cdn.example.com/portrait.jpg",
  "status": "Đang chờ duyệt",
  "rejectionReason": null,
  "verifiedAt": null,
  "rejectedAt": null,
  "createdAt": "2026-11-16T08:00:00Z",
  "sender": {
    "id": 24,
    "publicId": "00000000-0000-0000-0000-000000000024",
    "phone": "+12025550123",
    "isVerified": true,
    "verifiedAt": "2026-11-15T08:00:00Z",
    "profile": {
      "id": 24,
      "fullname": "Nguyễn Văn A",
      "gender": "Nam",
      "birthdate": "1998-05-20",
      "avatar": null
    },
    "currentIdentity": {
      "cardId": "012345678901",
      "fullNameOnCard": "NGUYEN VAN A",
      "cardFrontImage": "https://cdn.example.com/id-front.jpg",
      "cardBackImage": "https://cdn.example.com/id-back.jpg"
    }
  }
}
```

Mỗi item list trả thông tin giấy tờ, `status`, `rejectionReason`, `verifiedAt`, `rejectedAt`, `createdAt` và `sender`. Sender gồm `id`, `publicId`, `phone`, `fullname`, `avatar`; thông tin tên/ảnh lấy từ profile, nếu profile cũ chưa có thì `fullname` fallback theo tên trên thẻ.

```json
{
  "id": 1,
  "fullNameOnCard": "NGUYEN VAN A",
  "idNumber": "012345678901",
  "cardFrontImage": "https://cdn.example.com/id-front.jpg",
  "cardBackImage": "https://cdn.example.com/id-back.jpg",
  "portraitImage": "https://cdn.example.com/portrait.jpg",
  "status": "Đang chờ duyệt",
  "rejectionReason": null,
  "verifiedAt": null,
  "rejectedAt": null,
  "createdAt": "2026-11-16T08:00:00Z",
  "sender": {
    "id": 24,
    "publicId": "00000000-0000-0000-0000-000000000024",
    "phone": "+12025550123",
    "fullname": "Nguyễn Văn A",
    "avatar": null
  }
}
```

```json
{
  "status": "REJECTED",
  "rejectionReason": "Ảnh mặt sau CCCD chưa rõ thông tin."
}
```

Khi ACCEPTED, hệ thống ghi thời điểm xác thực cho request và user, tạo/cập nhật `profile_identities`, rồi thêm notification cùng recipient detail trong cùng transaction. Khi REJECTED, hệ thống ghi thời điểm/lý do từ chối trên request và user, rồi tạo notification cho người gửi. Chỉ yêu cầu PENDING được chuyển trạng thái; gửi lại cùng trạng thái là idempotent, trạng thái kết thúc khác không thể đổi ngược.

`user.verified_at` hiện là thời điểm xác thực số điện thoại qua OTP. Để không thay đổi ý nghĩa AuthModule, review giấy tờ dùng `user.identity_verified_at` và `user.identity_rejected_at`. Nếu một yêu cầu cũ không có Profile, thao tác ACCEPTED bị từ chối và transaction không ghi trạng thái hay notification một phần.

Migration `AddVerificationRequestAdminReview` bổ sung timestamp gửi/từ chối, thông tin trạng thái định danh user, Profile/Identity và các bảng notification cần cho luồng này.
