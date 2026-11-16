using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class VerificationAdminClaimsInvalidException()
    : AppException(
        StatusCodes.Status401Unauthorized,
        "Thông tin admin trong access token không hợp lệ.",
        "verification_admin_claims_invalid");
