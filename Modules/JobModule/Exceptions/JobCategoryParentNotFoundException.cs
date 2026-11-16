using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobCategoryParentNotFoundException()
    : AppException(
        StatusCodes.Status400BadRequest,
        "Không tìm thấy danh mục cha.",
        "job_category_parent_not_found");
