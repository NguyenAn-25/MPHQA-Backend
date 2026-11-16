using DotNetSkeleton.Shared.Exceptions;

namespace DotNetSkeleton.Modules.NotificationModule.Exceptions;

public sealed class NotificationNotFoundException()
    : AppException(StatusCodes.Status404NotFound, "Không tìm thấy thông báo.");
