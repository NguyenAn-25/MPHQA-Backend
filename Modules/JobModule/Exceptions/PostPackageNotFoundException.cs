using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class PostPackageNotFoundException(int id)
    : AppException(
        StatusCodes.Status404NotFound,
        $"Không tìm thấy gói đăng tuyển với mã {id}.",
        "post_package_not_found");
