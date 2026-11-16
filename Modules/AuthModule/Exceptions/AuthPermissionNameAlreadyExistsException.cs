using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.AuthModule.Exceptions;

public sealed class AuthPermissionNameAlreadyExistsException()
    : AppException(StatusCodes.Status409Conflict, "Tên nhóm quyền đã được sử dụng.");
