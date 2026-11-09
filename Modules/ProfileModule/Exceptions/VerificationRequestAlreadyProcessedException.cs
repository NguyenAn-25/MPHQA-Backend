using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class VerificationRequestAlreadyProcessedException()
    : AppException(
        StatusCodes.Status409Conflict,
        "Yêu cầu xác thực đã được xử lý và không thể đổi trạng thái.",
        "verification_request_already_processed");
