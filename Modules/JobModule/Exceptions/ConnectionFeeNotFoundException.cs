using DotNetSkeleton.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class ConnectionFeeNotFoundException()
    : AppException(
        StatusCodes.Status404NotFound,
        "Chưa có mức phí kết nối khả dụng tại thời điểm hiện tại.",
        "connection_fee_not_found");
