using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class JobCreationException(
    int statusCode,
    string message,
    string code)
    : AppException(statusCode, message, code);
