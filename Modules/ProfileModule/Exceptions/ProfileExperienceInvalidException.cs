using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class ProfileExperienceInvalidException()
    : AppException(
        StatusCodes.Status400BadRequest,
        "Danh sách kinh nghiệm không hợp lệ.",
        "profile_experience_invalid");
