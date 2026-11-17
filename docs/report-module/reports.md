# Quản lý báo cáo

`ReportModule` cung cấp API dành cho Admin để xem, đối chiếu và xử lý báo cáo. Danh sách và chi tiết yêu cầu role `ADMIN` cùng permission `reports:read`; xử lý yêu cầu role `ADMIN` cùng permission `reports:update`.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/admin/reports?page=1&pageSize=10&objectType=JOB&status=PENDING&createdFrom=...&createdTo=...` | `reports:read` | Danh sách report, có phân trang, lọc theo loại đối tượng, trạng thái và khoảng ngày tạo. |
| `GET /api/v1/admin/reports/{id}` | `reports:read` | Chi tiết report, ảnh minh chứng, người gửi và đối tượng được báo cáo. |
| `PATCH /api/v1/admin/reports/{id}/process` | `reports:update` | Chuyển report đang PENDING sang RESOLVED hoặc REJECTED, ghi thời gian và ghi chú xử lý. |

## Enum và bộ lọc

`objectType` và `status` được xử lý bằng enum cùng JSON, query-string và EF converter. Query/body nhận mã tiếng Anh hoặc nhãn tiếng Việt tương ứng; response trả nhãn tiếng Việt. Mã lưu trong database vẫn là chữ in hoa.

| Mã | Nhãn trong response |
|---|---|
| `JOB` | `Công việc` |
| `USER` | `Người dùng` |
| `PENDING` | `Đang chờ xử lý` |
| `RESOLVED` | `Đã xử lý` |
| `REJECTED` | `Đã từ chối` |
| `CANCELLED` | `Đã hủy` |

Mặc định danh sách trả về toàn bộ kết quả, không phân trang. Chỉ phân trang khi truyền `page` hoặc `pageSize`; nếu truyền `page` thì `pageSize` là bắt buộc, còn chỉ truyền `pageSize` thì `page` mặc định là 1. `pageSize` phải từ 1 đến 100. Kết quả sắp xếp theo `id` tăng dần. `createdFrom`/`createdTo` nhận ISO-8601, tính cả hai đầu mốc; nếu cả hai được truyền thì `createdFrom` phải nhỏ hơn hoặc bằng `createdTo`.

## Chi tiết đối tượng

`target.type` trùng với `objectType`. Với report loại JOB, `jobDetails` chứa ID, tiêu đề, trạng thái và thời gian tạo; `userDetails` là `null`. Với report loại USER, `userDetails` chứa ID, public ID, họ tên, số điện thoại và trạng thái hoạt động; `jobDetails` là `null`. Nếu đối tượng đã bị xóa, phần chi tiết đối tượng tương ứng trả `null`. `sender.fullname` là chuỗi rỗng nếu người gửi chưa có profile; `avatar` là `null` nếu chưa có ảnh.

## Xử lý báo cáo

Request:

```json
{
  "status": "RESOLVED | REJECTED",
  "note": "string (tùy chọn, tối đa 1000 ký tự)"
}
```

Khi xử lý thành công, hệ thống ghi `processed_at` và ghi chú, đồng thời tạo notification cho người gửi báo cáo cùng một notification detail. Chỉ report PENDING được chuyển trạng thái. Gửi lại cùng trạng thái là idempotent; yêu cầu đổi trạng thái đã kết thúc trả `409 Conflict`.

Migration `AddReportsModule` tạo bảng `reports`, các index phục vụ truy vấn và function/grant `REPORTS` cho Admin mặc định.

Trong môi trường Development, `ReportDevelopmentSampleDataSeeder` thêm 24 report nhắm đến user và tối đa 24 report nhắm đến job đang có trong database. Dữ liệu gồm đủ các trạng thái, có thể chạy lặp lại mà không tạo bản sao; report JOB được bỏ qua nếu chưa có job mẫu.
