# Hồ sơ cá nhân

Hồ sơ thuộc `ProfileModule`. Các API yêu cầu user đăng nhập; `userId` được lấy từ access token nên client không thể cập nhật hồ sơ hoặc gửi yêu cầu xác thực thay cho user khác.

| Method và route | Tác dụng |
|---|---|
| `GET /api/v1/profiles/me` | Lấy thông tin profile, identity, kinh nghiệm và kỹ năng của user đang đăng nhập. |
| `PUT /api/v1/profiles/me` | Cập nhật riêng thông tin trong `profiles`. Nếu user chưa có profile, API tạo mới. |
| `PUT /api/v1/profiles/me/skills` | Thay thế danh sách kỹ năng trong `profile_skills`. |
| `PUT /api/v1/profiles/me/experiences` | Thay thế danh sách kinh nghiệm trong `profile_experiences`. |
| `POST /api/v1/verification-requests` | Gửi giấy tờ định danh để Admin xét duyệt. |

Request cập nhật profile:

```json
{
  "fullname": "Nguyễn Văn A",
  "note": "Có thể làm việc cuối tuần",
  "major": "Công nghệ thông tin",
  "gender": "Nam",
  "avatar": "https://cdn.example.com/avatar.jpg",
  "birthdate": "2000-01-15",
  "address": "Quận 1, TP. Hồ Chí Minh",
  "longitude": 106.7009,
  "latitude": 10.7769
}
```

Request cập nhật kỹ năng:

```json
{
  "skillIds": [1, 5, 20]
}
```

Request cập nhật kinh nghiệm:

```json
{
  "experiences": [
    {
      "id": 1,
      "jobTitle": "Nhân viên phục vụ",
      "companyName": "Quán ăn ABC",
      "description": "Phục vụ khách và chuẩn bị bàn.",
      "title": "Phục vụ theo ca",
      "jobCategoryId": 20
    }
  ]
}
```

API kỹ năng thay thế danh sách `skillIds`; gửi `[]` để xóa toàn bộ kỹ năng. Với kinh nghiệm, truyền `id` ở từng phần tử để cập nhật bản ghi hiện có thuộc profile đang đăng nhập; bỏ `id` để thêm mới. Các kinh nghiệm không có trong request được giữ nguyên, và `experiences: []` không xóa dữ liệu. User cần có profile trước khi cập nhật kỹ năng hoặc kinh nghiệm; có thể tạo profile bằng `PUT /api/v1/profiles/me`. ID kỹ năng phải tồn tại và đang hoạt động. `jobCategoryId` nếu có phải tham chiếu lĩnh vực hiện có. Mỗi API chỉ cập nhật bảng tương ứng và vẫn trả về hồ sơ tổng hợp hiện tại.

Response profile có `identity` chứa `cardId`, `fullNameOnCard`, `cardFrontImage` và `cardBackImage`. Trường này là `null` khi chưa có identity được Admin xác nhận; sau khi yêu cầu xác thực được chấp nhận, API profile trả dữ liệu từ `profile_identities`.

Trong `experiences`, thông tin lĩnh vực được lồng trong `jobCategory` theo cấu trúc chi tiết lĩnh vực: `id`, `name`, `description`, `icon`, `isActive`, `parentId` và `parent` (nếu có). Trong `skills`, thông tin kỹ năng được lồng trong `skill` gồm `id`, `name`, `description`, `icon` và `isActive`.

Request gửi giấy tờ:

```json
{
  "fullNameOnCard": "NGUYEN VAN A",
  "idNumber": "012345678901",
  "cardFrontImage": "https://cdn.example.com/id-front.jpg",
  "cardBackImage": "https://cdn.example.com/id-back.jpg",
  "portraitImage": "https://cdn.example.com/portrait.jpg"
}
```

Yêu cầu mới có trạng thái `PENDING`; người gửi được xác định từ access token. Nếu user chưa có profile, API tạo profile tối thiểu bằng họ tên trên giấy tờ. Mỗi user chỉ được có một yêu cầu `PENDING` tại một thời điểm; gửi tiếp khi đang chờ sẽ trả `409 Conflict`.

Migration `AddProfileExperienceAndSkillLinks` tạo `profile_experiences`, `profile_skills`, khóa ngoại và ràng buộc một yêu cầu đang chờ trên mỗi user.
