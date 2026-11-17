using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.ReportModule.Exceptions;

public sealed class ReportAlreadyProcessedException()
    : AppException(
        StatusCodes.Status409Conflict,
        "Báo cáo đã được xử lý và không thể đổi trạng thái.",
        "report_already_processed");
