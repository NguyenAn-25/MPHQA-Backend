# CORS cho frontend production

API đăng ký policy `Frontend` trong `Program.cs` và gọi `UseCors` ngay sau `UseRouting`. Cấu hình origin được đọc từ `Cors:AllowedOrigins`.

Trong production, khai báo origin đầy đủ của frontend bằng environment variables:

```text
ASPNETCORE_ENVIRONMENT=Production
Cors__AllowedOrigins__0=https://frontend.example.com
Cors__AllowedOrigins__1=https://admin.example.com
```

Chỉ khai báo scheme, host và port của trang frontend, không thêm path hay dấu `/` cuối. Dùng đúng origin mà trình duyệt gửi trong header `Origin`; ví dụ local có thể là `http://localhost:5173`, production thường là `https://...`. Nếu chỉ có một frontend thì chỉ cần biến `__0`.

Sau khi cập nhật environment variables, khởi động lại/redeploy API. Các request preflight `OPTIONS` được policy xử lý; policy cho phép mọi HTTP method và header để hỗ trợ JSON và bearer token. Policy không bật cookie credentials. Nếu xác thực bằng cookie, cần cấu hình CORS credentials và cookie riêng theo domain frontend.

Không dùng `*` cho origin production. `appsettings.Production.json` mặc định để danh sách rỗng, vì vậy frontend sẽ không được trình duyệt cho phép gọi API cho đến khi origin production được cấu hình. `AllowedHosts` không thay thế CORS.
