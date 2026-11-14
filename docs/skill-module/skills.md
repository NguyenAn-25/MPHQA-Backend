# Quản lý kỹ năng (Skill)

`SkillModule` quản lý danh mục kỹ năng dùng chung cho cả job và hồ sơ người dùng. `SkillEntity` nằm trong module riêng; `JobRequiredSkillEntity` và `ProfileSkillEntity` tham chiếu entity này.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/skills?page=1&pageSize=10&search=Excel&isActive=true` | `skills:read` | Danh sách summary có phân trang, lọc theo tên và trạng thái. |
| `GET /api/v1/skills/{id}` | `skills:read` | Chi tiết một kỹ năng. |
| `GET /api/v1/skills/options` | Không yêu cầu | Danh sách gọn `id`, `name` của kỹ năng đang hoạt động để chọn khi tạo job/hồ sơ. |
| `POST /api/v1/skills` | `skills:create` | Tạo kỹ năng. |
| `PUT /api/v1/skills/{id}` | `skills:update` | Sửa tên, mô tả và icon. |
| `PATCH /api/v1/skills/{id}/status` | `skills:update` | Ẩn hoặc hiện kỹ năng qua `isActive`. |
| `DELETE /api/v1/skills/{id}` | `skills:delete` | Xóa nếu chưa được job hoặc hồ sơ sử dụng. |

Request tạo mới:

```json
{
  "name": "Pha chế đồ uống",
  "description": "Chuẩn bị cà phê và các loại thức uống theo công thức.",
  "icon": null
}
```

Summary trả `items`, `page`, `pageSize`, `totalCount` với các trường `id`, `name`, `icon`, `isActive`. Detail bổ sung `description`. Endpoint `options` chỉ trả kỹ năng đang hoạt động. Xóa kỹ năng đang được `job_required_skills` hoặc `profile_skills` tham chiếu trả lỗi `409`.

Migration `AddSkillManagement` tạo bảng `skills` và function/permission tương ứng. Trong Development, `SkillSeeder` thêm 157 kỹ năng liên quan đến công việc ngắn hạn, chỉ thêm tên còn thiếu.
