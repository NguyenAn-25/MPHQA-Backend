using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ProfileModule.Exceptions;

public sealed class ProfileJobCategoryUnavailableException()
    : AppException(
        StatusCodes.Status400BadRequest,
        "Một hoặc nhiều lĩnh vực kinh nghiệm không tồn tại.",
        "profile_job_category_unavailable");
