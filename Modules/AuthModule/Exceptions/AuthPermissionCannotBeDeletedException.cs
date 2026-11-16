using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.AuthModule.Exceptions;

public sealed class AuthPermissionCannotBeDeletedException()
    : AppException(StatusCodes.Status409Conflict, "Không thể xóa nhóm quyền đang được tài khoản sử dụng.");
