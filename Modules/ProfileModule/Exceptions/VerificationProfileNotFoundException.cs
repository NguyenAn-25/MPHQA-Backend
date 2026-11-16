using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class VerificationProfileNotFoundException()
    : AppException(
        StatusCodes.Status409Conflict,
        "Người gửi chưa có hồ sơ để lưu thông tin định danh đã xác thực.",
        "verification_profile_not_found");
