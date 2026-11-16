using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.NotificationModule.Exceptions;

public sealed class NotificationRecipientNotFoundException()
    : AppException(StatusCodes.Status404NotFound, "Không tìm thấy người nhận phù hợp.");
