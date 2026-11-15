using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobNotFoundByIdException(int id)
    : AppException(
        StatusCodes.Status404NotFound,
        $"Không tìm thấy công việc có Id = {id}.",
        "job_not_found");
