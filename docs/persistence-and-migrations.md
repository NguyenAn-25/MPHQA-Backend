# Cơ sở dữ liệu và migration

## Vai trò của persistence layer

Persistence dùng EF Core với provider PostgreSQL Npgsql:

- `Shared/Configurations/DatabaseConfiguration.cs` đăng ký `AppDbContext` qua `AddDbContext` và `UseNpgsql`.
- `Shared/Persistence/AppDbContext.cs` là điểm tập trung model EF/DbSet của module.
- `Shared/Persistence/Migrations/` chứa migration, metadata và `AppDbContextModelSnapshot`.
- `Shared/Persistence/AppDbContextFactory.cs` cho phép EF CLI tạo context mà không start web app hoặc chạy seeder.

Entity/tables thuộc domain vẫn nằm trong module tương ứng; migration được tập trung dưới `Shared/Persistence/Migrations`.

## Tạo kết nối và EF tool

Local tool manifest `.config/dotnet-tools.json` khóa `dotnet-ef` 10.0.14. Chạy từ thư mục project:

```bash
dotnet tool restore
dotnet tool run dotnet-ef -- migrations list
```

Factory đọc appsettings, User Secrets và environment variables. Nếu không thấy `ConnectionStrings:Default`, factory dùng fallback local `Host=localhost;Database=dotnet_skeleton;Username=postgres;Password=postgres`. Trước khi chạy lệnh ghi database, hãy xác nhận connection string trỏ đúng database đích.

## Tạo và áp dụng migration

Sau khi thay đổi entity/model, tạo migration:

```bash
dotnet tool run dotnet-ef -- migrations add TenMigration
```

Review migration và `AppDbContextModelSnapshot.cs`; sau đó áp dụng ở local:

```bash
dotnet tool run dotnet-ef -- database update
```

Trong Development, `Program.cs` gọi `SeedDatabaseAsync()`. `DatabaseSeeder.RunAsync()` chạy `Database.MigrateAsync()` trước khi gọi các seeder. Vì vậy khi chạy ứng dụng local, schema được cập nhật tự động.

## Cách triển khai Production

Production startup hiện không tự chạy migration và seeder. Migration nên là một bước release riêng:

```bash
dotnet tool run dotnet-ef -- migrations script --idempotent --output migration.sql
```

Review SQL script, xác nhận database đích/backup, chạy migration trước khi bật phiên bản app mới. Không cho nhiều replica cùng tự migrate lúc khởi động nếu không có cơ chế điều phối.

## Seeder pipeline

`ISeeder` có `Order` và `SeedAsync(AppDbContext)`. `DatabaseSeeder` sắp xếp theo `Order`, chạy lần lượt, ghi số bản ghi thêm và dừng/ghi log nếu seeder lỗi. Thứ tự hữu ích khi có khóa ngoại giữa dữ liệu danh mục và dữ liệu tham chiếu.

`SeederConfiguration.AddSeeders()` hiện đăng ký catalog và các seeder Development cho Auth, danh mục lĩnh vực, kỹ năng, profile, job, notification và report; `ExampleDataSeeder` có trong source nhưng chưa đăng ký. Pipeline seeder được gọi khi ứng dụng ở Development. `JobCategorySeeder` tạo 28 lĩnh vực gốc và 181 danh mục việc làm ngắn hạn; `SkillSeeder` tạo 157 kỹ năng; `JobDevelopmentSampleDataSeeder` thêm tối đa 120 job cùng ảnh, kỹ năng yêu cầu và ứng viên. Các danh mục chỉ thêm mục còn thiếu; seeder mẫu chỉ ghi dữ liệu trong Development. `AuthDevelopmentAdminAccountsSeeder` tạo hai tài khoản ADMIN fixture có số `+12025550190` và `+12025550191`, mật khẩu `123456`; `AuthDevelopmentAdminSeeder` có thể tạo thêm user khi có đủ `Seed:AdminPhone` cùng `Seed:AdminPassword`.

`AuthPermissionCatalogSeeder` tìm function/permission theo business key và đồng bộ function `users`, `job_categories`, `skills`, `verification_requests`, `notifications`, `permissions` cùng profile `ADMIN_DEFAULT`. Grant khởi tạo chỉ được thêm khi profile được tạo lần đầu; migration mới bổ sung grant cho profile mặc định đã tồn tại, còn lần chạy seeder sau không tự thêm lại grant đã bị gỡ. `AuthDevelopmentSampleDataSeeder` tạo tối đa 60 user fixture thường với OTP và session lịch sử đã hết hạn. `ProfileDevelopmentSampleDataSeeder` bổ sung hồ sơ, kinh nghiệm, kỹ năng và verification request mẫu có liên kết hợp lệ. Các tài khoản mẫu có password Development `123456`; seeder đặt lại password này nếu fixture đã tồn tại, nhưng không nhân bản account hoặc lịch sử. Các tài khoản ADMIN fixture và dữ liệu user mẫu chỉ dùng ở Development; startup Production không gọi runtime seeder.

## Migration hiện có

Migration AuthModule mới nhất là `20261101094107_LimitOtpVerificationAttempts`, bổ sung bộ đếm số lần nhập OTP sai. Mô hình sáu bảng của module nằm ở [Mô hình dữ liệu AuthModule](auth-module/data-model-and-migrations.md). Các migration cũ của project cũng nằm chung trong thư mục migration.

Migration `AddProfileExperienceAndSkillLinks` tạo các bảng `profile_experiences` và `profile_skills`, cùng khóa ngoại sang profile, lĩnh vực và kỹ năng; migration cũng giới hạn một yêu cầu xác thực `PENDING` trên mỗi user. Migration `AddPermissionProfileManagement` thêm function/action quản lý nhóm quyền và grant mặc định cho `ADMIN_DEFAULT`; `RemoveLegacyPermissionProfilesFunction` gộp grant của function cũ vào `permissions`, thu hồi session liên quan và xóa function trùng lặp. Migration `AddVerificationRequestAdminReview` tạo Profile/Identity, verification requests, trạng thái review trên user và notification tables. `AddSkillManagement` tạo bảng dùng chung `skills`; `AddJobCategoryManagement` tạo `job_categories`; `AddJobModerationModule` tạo bảng công việc, ứng tuyển, ảnh, kỹ năng cần thiết, gói đăng, mục đã lưu và đánh giá.

`Down()` của migration AuthModule hiện xóa các bảng AuthModule và dữ liệu liên quan. Chỉ rollback sau khi xác định rõ dữ liệu cần giữ và có bản sao lưu phù hợp.
