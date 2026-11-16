using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobCategoryCannotBeDeletedException()
    : AppException(
        StatusCodes.Status409Conflict,
        "Không thể xóa lĩnh vực đang được job, kinh nghiệm hoặc danh mục con sử dụng.",
        "job_category_in_use");
