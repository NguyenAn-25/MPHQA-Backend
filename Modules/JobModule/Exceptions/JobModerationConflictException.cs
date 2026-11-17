using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobModerationConflictException()
    : AppException(StatusCodes.Status409Conflict, "Trạng thái hiện tại của công việc không cho phép thực hiện chuyển trạng thái này.");
