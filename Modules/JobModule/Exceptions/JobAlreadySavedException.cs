using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobAlreadySavedException(int id)
    : AppException(
        StatusCodes.Status409Conflict,
        $"Công việc có Id = {id} đã được lưu.",
        "job_already_saved");
