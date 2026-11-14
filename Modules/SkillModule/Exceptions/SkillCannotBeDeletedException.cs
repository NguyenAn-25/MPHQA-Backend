using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.SkillModule.Exceptions;

public sealed class SkillCannotBeDeletedException()
    : AppException(
        StatusCodes.Status409Conflict,
        "Không thể xóa kỹ năng đang được job hoặc hồ sơ sử dụng.",
        "skill_in_use");
