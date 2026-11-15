using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class ConnectionFeeSettingCreationException(
    int statusCode,
    string message,
    string code)
    : AppException(statusCode, message, code);
