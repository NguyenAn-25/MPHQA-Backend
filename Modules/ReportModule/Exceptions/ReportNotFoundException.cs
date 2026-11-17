using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ReportModule.Exceptions;

public sealed class ReportNotFoundException()
    : AppException(StatusCodes.Status404NotFound, "Không tìm thấy báo cáo.");
