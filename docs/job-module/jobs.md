# Quản lý công việc

Các API quản trị công việc yêu cầu role `ADMIN` cùng permission `jobs:read` cho truy vấn và `jobs:update` cho cập nhật trạng thái.

| Method và route | Permission | Mô tả |
|---|---|---|
| `GET /api/v1/admin/jobs` | `jobs:read` | Lấy danh sách, lọc theo tiêu đề, trạng thái, mô hình tính phí, danh mục, người đăng và khoảng ngày tạo. |
| `GET /api/v1/admin/jobs/{id}` | `jobs:read` | Lấy chi tiết job, danh mục, gói đăng, người đăng, ảnh và kỹ năng yêu cầu. |
| `PATCH /api/v1/admin/jobs/{id}/status` | `jobs:update` | Chuyển trạng thái sang `ACCEPTED`, `REJECTED` hoặc `CANCELLED`. |

## Danh sách công việc

Query parameters:

```json
{
  "page": "integer (tùy chọn)",
  "pageSize": "integer (tùy chọn, từ 1 đến 100)",
  "search": "string (tùy chọn, tìm trong tiêu đề)",
  "status": "PENDING | ACCEPTED | WORKING | COMPLETED | REJECTED | CANCELLED hoặc nhãn tiếng Việt (tùy chọn)",
  "pricingModel": "TIME_BASED | PER_CONNECTION hoặc nhãn tiếng Việt (tùy chọn)",
  "categoryId": "integer (tùy chọn)",
  "createdBy": "integer (tùy chọn)",
  "createdFrom": "ISO-8601 (tùy chọn)",
  "createdTo": "ISO-8601 (tùy chọn)"
}
```

Mặc định API trả toàn bộ kết quả, không phân trang. Khi truyền `page` thì phải truyền kèm `pageSize`; nếu chỉ truyền `pageSize`, trang mặc định là 1. `pageSize` phải từ 1 đến 100. Danh sách sắp xếp theo `id` tăng dần. Khoảng ngày tạo bao gồm cả hai mốc.

Response `data`:

```json
{
  "items": [
    {
      "id": "integer",
      "title": "string",
      "salaryValue": "decimal",
      "salaryUnit": "string",
      "pricingModel": "Theo thời gian | Theo lượt kết nối",
      "status": "PENDING | ACCEPTED | WORKING | COMPLETED | REJECTED | CANCELLED",
      "applicantCount": "integer",
      "createdAt": "ISO-8601",
      "category": { "id": "integer", "name": "string" },
      "author": {
        "id": "integer",
        "publicId": "string",
        "phone": "string",
        "fullname": "string",
        "avatar": "string | null"
      }
    }
  ],
  "page": "integer",
  "pageSize": "integer",
  "totalCount": "integer"
}
```

## Chi tiết công việc

`GET /api/v1/admin/jobs/{id}` không nhận body. `data` chứa toàn bộ trường job, các mốc trạng thái, `category`, `postPackage` (có thể `null`), `author`, `images` và `requiredSkills`. `requiredSkills[].id` là ID của skill.

## Cập nhật trạng thái

Request:

```json
{
  "status": "ACCEPTED | REJECTED | CANCELLED hoặc nhãn tiếng Việt (bắt buộc)",
  "rejectionReason": "string (tùy chọn, chỉ dùng với REJECTED, tối đa 1000 ký tự)",
  "cancelReason": "string (bắt buộc với CANCELLED, tối đa 1000 ký tự)"
}
```

Response `data`:

```json
{
  "id": "integer",
  "status": "Đã duyệt | Đã từ chối | Đã hủy",
  "acceptedAt": "ISO-8601 | null",
  "rejectedAt": "ISO-8601 | null",
  "cancelledAt": "ISO-8601 | null",
  "rejectionReason": "string | null",
  "cancelReason": "string | null"
}
```

`ACCEPTED` và `REJECTED` chỉ áp dụng khi job đang `PENDING`. `CANCELLED` áp dụng cho job `PENDING`, `ACCEPTED` hoặc `WORKING`. Khi đổi trạng thái thành công, hệ thống gửi notification cho người đăng. Trạng thái đã kết thúc không thể đổi sang trạng thái khác.

Trong môi trường Development, `JobDevelopmentSampleDataSeeder` thêm tối đa 120 job ngắn hạn, cùng post package mẫu, ảnh địa điểm, kỹ năng cần có và các ứng viên. Seeder chỉ thêm job có tiêu đề fixture chưa tồn tại; các report fixture chạy sau sẽ tham chiếu job mẫu này.

## Job fixture để thử cập nhật trạng thái

Sau khi seeder chạy, lọc danh sách bằng `GET /api/v1/admin/jobs?search=API%20STATUS%20TEST` để tìm các job có tiêu đề `[DEV JOB] API STATUS TEST ...`. ID được sinh theo dữ liệu trong database, vì vậy lấy `id` từ response danh sách trước khi gọi `PATCH /api/v1/admin/jobs/{id}/status`.

| Tên job chứa | Trạng thái ban đầu | Request để thử |
|---|---|---|
| `PENDING TO ACCEPTED` | `Đang chờ duyệt` | `{"status":"ACCEPTED"}` — thành công |
| `PENDING TO REJECTED` | `Đang chờ duyệt` | `{"status":"REJECTED","rejectionReason":"Thông tin tuyển dụng chưa rõ"}` — thành công |
| `PENDING TO CANCELLED` | `Đang chờ duyệt` | `{"status":"CANCELLED","cancelReason":"Đơn vị dừng tuyển"}` — thành công |
| `ACCEPTED TO CANCELLED` | `Đã duyệt` | `{"status":"CANCELLED","cancelReason":"Hủy ca đã đăng"}` — thành công |
| `WORKING TO CANCELLED` | `Đang làm` | `{"status":"CANCELLED","cancelReason":"Gỡ bài vi phạm"}` — thành công |
| `REJECTED CONFLICT` | `Đã từ chối` | `{"status":"ACCEPTED"}` — `409 Conflict` |
| `COMPLETED CONFLICT` | `Đã hoàn thành` | `{"status":"CANCELLED","cancelReason":"Gỡ bài"}` — `409 Conflict` |
| `CANCELLED IDEMPOTENT` | `Đã hủy` | Gửi lại `CANCELLED` — trả kết quả hiện tại, không tạo notification mới |
| `ACCEPTED IDEMPOTENT` | `Đã duyệt` | Gửi lại `ACCEPTED` — trả kết quả hiện tại, không tạo notification mới |
| `WORKING CONFLICT` | `Đang làm` | `{"status":"ACCEPTED"}` — `409 Conflict` |
