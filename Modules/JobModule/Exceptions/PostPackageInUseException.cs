using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class PostPackageInUseException(int id)
    : AppException(
        StatusCodes.Status409Conflict,
        $"Không thể xóa gói đăng tuyển với mã {id} vì đã có job sử dụng.",
        "post_package_in_use");
