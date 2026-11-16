using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.AuthModule.Exceptions;

public sealed class AuthPermissionNotFoundException()
    : AppException(StatusCodes.Status404NotFound, "Không tìm thấy nhóm quyền.");
