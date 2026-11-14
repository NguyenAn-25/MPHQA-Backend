# Xử lý yêu cầu xác thực

`ProfileModule` cung cấp các API dành cho admin xử lý yêu cầu xác thực giấy tờ cùng với các chức năng hồ sơ. API dùng permission của AuthModule; profile `ADMIN_DEFAULT` được cấp `verification_requests:read` và `verification_requests:update`.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/verification-requests?page=1&pageSize=10&status=PENDING&submittedFrom=...&submittedTo=...` | `verification_requests:read` | Danh sách summary có phân trang, lọc theo trạng thái và thời gian gửi. |
| `GET /api/v1/verification-requests/{id}` | `verification_requests:read` | Xem giấy tờ, ảnh chân dung, người gửi và dữ liệu Profile/Identity để đối chiếu. |
| `PATCH /api/v1/verification-requests/{id}/status` | `verification_requests:update` | Chấp nhận hoặc từ chối yêu cầu đang PENDING. |

Trạng thái list nhận `PENDING`, `ACCEPTED`, `REJECTED` hoặc `CANCELLED`. `submittedFrom` và `submittedTo` là ISO 8601, inclusive. Request từ chối phải có lý do:

```json
{
  "status": "REJECTED",
  "rejectionReason": "Ảnh mặt sau CCCD chưa rõ thông tin."
}
```

Khi ACCEPTED, hệ thống ghi thời điểm xác thực cho request và user, tạo/cập nhật `profile_identities`, rồi thêm notification cùng recipient detail trong cùng transaction. Khi REJECTED, hệ thống ghi thời điểm/lý do từ chối trên request và user, rồi tạo notification cho người gửi. Chỉ yêu cầu PENDING được chuyển trạng thái; gửi lại cùng trạng thái là idempotent, trạng thái kết thúc khác không thể đổi ngược.

`user.verified_at` hiện là thời điểm xác thực số điện thoại qua OTP. Để không thay đổi ý nghĩa AuthModule, review giấy tờ dùng `user.identity_verified_at` và `user.identity_rejected_at`. ACCEPTED yêu cầu người gửi đã có Profile; nếu chưa có, toàn bộ giao dịch bị từ chối và không ghi notification.

Migration `AddVerificationRequestAdminReview` bổ sung timestamp gửi/từ chối, thông tin trạng thái định danh user, Profile/Identity và các bảng notification cần cho luồng này.
