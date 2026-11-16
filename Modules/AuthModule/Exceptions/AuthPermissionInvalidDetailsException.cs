using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.AuthModule.Exceptions;

public sealed class AuthPermissionInvalidDetailsException(string message)
    : AppException(StatusCodes.Status400BadRequest, message, "invalid_permission_details");
