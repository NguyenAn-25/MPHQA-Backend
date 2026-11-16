using DotNetSkeleton.Modules.NotificationModule.Enums;

namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;

public sealed record NotificationDetailResponseDto(
    int Id,
    string Title,
    string Content,
    NotificationSendTo SendTo,
    DateTimeOffset CreatedAt,
    NotificationDetailCreatorResponseDto Creator,
    PagedNotificationRecipientsResponseDto Recipients);
