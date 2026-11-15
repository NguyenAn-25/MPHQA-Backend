using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobNotSavedException(int id)
    : AppException(
        StatusCodes.Status404NotFound,
        $"Công việc có Id = {id} chưa được lưu.",
        "job_not_saved");
