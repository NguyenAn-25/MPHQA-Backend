# Quản lý kỹ năng (Skill)

`SkillModule` quản lý danh mục kỹ năng dùng chung cho cả job và hồ sơ người dùng. `SkillEntity` nằm trong module riêng; `JobRequiredSkillEntity` và `ProfileSkillEntity` tham chiếu entity này. Các endpoint quản trị yêu cầu role `ADMIN` cùng permission tương ứng; `options` vẫn công khai.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/skills?search=Excel&isActive=true` | `skills:read` | Danh sách summary đầy đủ, lọc theo tên và trạng thái. |
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

Danh sách summary và `options` được sắp xếp theo `id` tăng dần.

Nếu không truyền `page` và `pageSize`, API trả toàn bộ kết quả phù hợp bộ lọc; response vẫn có metadata với `page=1` và `pageSize` bằng số item trả về. Truyền một trong hai tham số sẽ bật phân trang; tham số còn thiếu mặc định là `page=1` hoặc `pageSize=10`.

Migration `AddSkillManagement` tạo bảng `skills` và function/permission tương ứng. Trong Development, `SkillSeeder` thêm 157 kỹ năng liên quan đến công việc ngắn hạn, chỉ thêm tên còn thiếu.
