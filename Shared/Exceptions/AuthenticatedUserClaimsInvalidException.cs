namespace DotNetSkeleton.Shared.Exceptions;

public sealed class AuthenticatedUserClaimsInvalidException()
    : AppException(
        StatusCodes.Status401Unauthorized,
        "Token thiếu thông tin người dùng hợp lệ.");
