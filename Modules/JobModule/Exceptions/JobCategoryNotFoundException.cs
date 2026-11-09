using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobCategoryNotFoundException()
    : AppException(
        StatusCodes.Status404NotFound,
        "Không tìm thấy lĩnh vực.");
