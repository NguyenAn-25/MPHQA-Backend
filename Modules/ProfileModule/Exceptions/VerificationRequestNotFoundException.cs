using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class VerificationRequestNotFoundException()
    : AppException(
        StatusCodes.Status404NotFound,
        "Không tìm thấy yêu cầu xác thực.");
