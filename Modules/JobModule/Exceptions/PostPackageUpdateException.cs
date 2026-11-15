using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class PostPackageUpdateException()
    : AppException(
        StatusCodes.Status400BadRequest,
        "Phải cung cấp ít nhất một trường dữ liệu để cập nhật gói đăng tuyển.",
        "post_package_update_empty");
