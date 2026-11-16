using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobAlreadyAppliedException(int id)
    : AppException(
        StatusCodes.Status409Conflict,
        $"Bạn đã ứng tuyển công việc có Id = {id}.",
        "job_already_applied");
