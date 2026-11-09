# Quản lý lĩnh vực (Job Category)

Các endpoint quản trị lĩnh vực nằm dưới `/api/v1/job-categories`, yêu cầu access token và permission tương ứng trong AuthModule; profile `ADMIN_DEFAULT` được cấp các quyền này. Endpoint `options` là dữ liệu tra cứu nhẹ cho client, không yêu cầu đăng nhập.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/job-categories?page=1&pageSize=10&search=phục vụ&isActive=true` | `job_categories:read` | Trả summary có phân trang theo lĩnh vực gốc; mỗi lĩnh vực gốc kèm cây danh mục con. |
| `GET /api/v1/job-categories/{id}` | `job_categories:read` | Trả một lĩnh vực cùng các danh mục con bên dưới. |
| `GET /api/v1/job-categories/options` | Không yêu cầu | Trả các lĩnh vực đang hoạt động dưới dạng gọn `id`, `name`, `parentId` để chọn khi tạo job/hồ sơ. |
| `POST /api/v1/job-categories` | `job_categories:create` | Tạo lĩnh vực gốc hoặc danh mục con. |
| `PUT /api/v1/job-categories/{id}` | `job_categories:update` | Sửa tên, mô tả, icon và danh mục cha. |
| `PATCH /api/v1/job-categories/{id}/status` | `job_categories:update` | Cập nhật `isActive` để ẩn hoặc hiện lĩnh vực. |
| `DELETE /api/v1/job-categories/{id}` | `job_categories:delete` | Xóa khi chưa có job, kinh nghiệm hồ sơ hoặc danh mục con tham chiếu. |

Request tạo mới:

```json
{
  "name": "Công nghệ thông tin",
  "description": "Các công việc trong lĩnh vực công nghệ thông tin.",
  "icon": "code",
  "parentId": null
}
```

Khi tạo danh mục con, `parentId` phải trỏ tới một lĩnh vực hiện có. Khi cập nhật, có thể đặt `parentId` thành `null` để chuyển lĩnh vực về gốc. Service từ chối đặt danh mục làm cha của chính nó hoặc hậu duệ của nó.

Request ẩn lĩnh vực:

```json
{
  "isActive": false
}
```

List hỗ trợ `page`, `pageSize` (mặc định `1`/`10`, tối đa 100 mục), `search` (tìm trong tên), `isActive` và `parentId`. Khi bỏ `parentId`, phân trang danh mục gốc; khi truyền `parentId`, phân trang các danh mục con trực tiếp của parent đó. Lọc search/trạng thái áp dụng cho các mục ở cấp đang phân trang; mỗi mục vẫn kèm toàn bộ cây con. `totalCount` là số mục khớp bộ lọc ở cấp đó.

Summary trả `items`, `page`, `pageSize`, `totalCount`; mỗi item gồm `id`, `name`, `icon`, `isActive`, `parentId`, `children`. Detail trả đúng một lĩnh vực với `description` và `parentName`. `options` trả danh sách phẳng; `parentId` giữ quan hệ cha con và các mục ẩn (cùng hậu duệ của chúng) không được trả.

Thành công dùng response envelope chung; tạo mới trả `201 Created`, xóa trả `204 No Content`. Lỗi không tìm thấy trả `404`, parent không tồn tại hoặc vòng lặp phân cấp trả `400`, và xóa danh mục đang được sử dụng trả `409`.

Schema và grant mặc định nằm trong migration `AddJobCategoryManagement`. Production cần áp dụng migration theo quy trình tại [Cơ sở dữ liệu và migration](../persistence-and-migrations.md).

Trong Development, `JobCategorySeeder` tạo 28 lĩnh vực gốc và 181 danh mục công việc ngắn hạn. Seeder chỉ thêm mục còn thiếu, giữ nguyên các thay đổi mà quản trị viên đã lưu. Pipeline hiện chạy seeder khi ứng dụng ở môi trường Development.
