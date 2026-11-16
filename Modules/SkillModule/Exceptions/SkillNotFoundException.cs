using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.SkillModule.Exceptions;

public sealed class SkillNotFoundException()
    : AppException(
        StatusCodes.Status404NotFound,
        "Không tìm thấy kỹ năng.");
