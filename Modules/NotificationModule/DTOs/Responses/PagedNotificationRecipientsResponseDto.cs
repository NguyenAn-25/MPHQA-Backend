namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;

public sealed record PagedNotificationRecipientsResponseDto(
    IReadOnlyList<NotificationRecipientResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int ReadCount);
