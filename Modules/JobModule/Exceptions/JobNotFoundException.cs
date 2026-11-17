using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobNotFoundException() : AppException(StatusCodes.Status404NotFound, "Không tìm thấy công việc.");
