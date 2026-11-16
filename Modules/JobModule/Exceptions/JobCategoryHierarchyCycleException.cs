using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobCategoryHierarchyCycleException()
    : AppException(
        StatusCodes.Status400BadRequest,
        "Không thể đặt danh mục cha là chính lĩnh vực này hoặc danh mục con của nó.",
        "job_category_hierarchy_cycle");
