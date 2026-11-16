using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class VerificationRequestPendingException()
    : AppException(
        StatusCodes.Status409Conflict,
        "Bạn đang có một yêu cầu xác thực chờ xét duyệt.",
        "verification_request_pending");
