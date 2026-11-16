using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobNotAcceptingApplicationsException(int id)
    : AppException(
        StatusCodes.Status409Conflict,
        $"Công việc có Id = {id} hiện không nhận ứng tuyển.",
        "job_not_accepting_applications");
