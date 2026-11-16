using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class ProfileSkillUnavailableException()
    : AppException(
        StatusCodes.Status400BadRequest,
        "Một hoặc nhiều kỹ năng không tồn tại.",
        "profile_skill_unavailable");
