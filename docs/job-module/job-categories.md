# Quản lý lĩnh vực (Job Category)

Các endpoint quản trị lĩnh vực nằm dưới `/api/v1/job-categories`, yêu cầu access token, role `ADMIN` và permission tương ứng trong AuthModule; profile `ADMIN_DEFAULT` được cấp các quyền này. Endpoint `options` là dữ liệu tra cứu nhẹ cho client, không yêu cầu đăng nhập.

| Method và route | Permission | Tác dụng |
|---|---|---|
| `GET /api/v1/job-categories?search=phục vụ&isActive=true` | `job_categories:read` | Trả đầy đủ summary theo lĩnh vực gốc; mỗi lĩnh vực gốc kèm cây danh mục con. |
| `GET /api/v1/job-categories/{id}` | `job_categories:read` | Trả một lĩnh vực cùng cây danh mục con bên dưới; mỗi nút gồm `id`, `name`, `icon`, `isActive`, `parentId`, `children`. |
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

List hỗ trợ `page`, `pageSize` (tối đa 100 mục), `search` (tìm trong tên), `isActive` và `parentId`. Khi bỏ `parentId`, danh sách ở cấp gốc; khi truyền `parentId`, danh sách gồm các danh mục con trực tiếp của parent đó. Lọc search/trạng thái áp dụng cho các mục ở cấp đang lấy; mỗi mục vẫn kèm toàn bộ cây con. `totalCount` là số mục khớp bộ lọc ở cấp đó. Không truyền cả `page` và `pageSize` thì trả toàn bộ kết quả; truyền ít nhất một tham số sẽ bật phân trang, tham số còn thiếu mặc định là `page=1` hoặc `pageSize=10`.

Summary của list và `GET /api/v1/job-categories/{id}` gồm `id`, `name`, `icon`, `isActive`, `parentId`, `children`; endpoint chi tiết trả cây bắt đầu từ lĩnh vực có ID yêu cầu. Create/update/status trả chi tiết lĩnh vực gồm `description` và object `parent` (nếu có), với `id`, `name`, `description`, `icon`, `isActive` và `parentId`. `options` trả danh sách phẳng; `parentId` giữ quan hệ cha con và các mục ẩn (cùng hậu duệ của chúng) không được trả.

Danh sách ở từng cấp và `options` được sắp xếp theo `id` tăng dần.

Thành công dùng response envelope chung; tạo mới trả `201 Created`, xóa trả `204 No Content`. Lỗi không tìm thấy trả `404`, parent không tồn tại hoặc vòng lặp phân cấp trả `400`, và xóa danh mục đang được sử dụng trả `409`.

Schema và grant mặc định nằm trong migration `AddJobCategoryManagement`. Production cần áp dụng migration theo quy trình tại [Cơ sở dữ liệu và migration](../persistence-and-migrations.md).

Trong Development, `JobCategorySeeder` tạo 28 lĩnh vực gốc và 181 danh mục công việc ngắn hạn. Seeder chỉ thêm mục còn thiếu, giữ nguyên các thay đổi mà quản trị viên đã lưu. Pipeline hiện chạy seeder khi ứng dụng ở môi trường Development.
