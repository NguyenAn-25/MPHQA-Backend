using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.NotificationModule.Exceptions;

public sealed class NotificationRequestInvalidException(string message)
    : AppException(StatusCodes.Status400BadRequest, message, "invalid_notification_request");
