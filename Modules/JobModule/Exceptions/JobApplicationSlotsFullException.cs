using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobApplicationSlotsFullException(int id)
    : AppException(
        StatusCodes.Status409Conflict,
        $"Công việc có Id = {id} đã đủ số lượng ứng viên cần tuyển.",
        "job_application_slots_full");
