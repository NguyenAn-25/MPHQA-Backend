using DotNetSkeleton.Modules.NotificationModule.Enums;

namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;

public sealed record NotificationSummaryResponseDto(
    int Id,
    string Title,
    string Content,
    NotificationSendTo SendTo,
    int TotalRecipients,
    int ReadCount,
    DateTimeOffset CreatedAt,
    NotificationListCreatorResponseDto Creator);
