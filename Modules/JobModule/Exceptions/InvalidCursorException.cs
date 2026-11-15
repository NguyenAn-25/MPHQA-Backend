using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.JobModule.Exceptions;

public sealed class InvalidCursorException()
    : AppException(
        StatusCodes.Status400BadRequest,
        "Cursor không hợp lệ.",
        "invalid_cursor");
