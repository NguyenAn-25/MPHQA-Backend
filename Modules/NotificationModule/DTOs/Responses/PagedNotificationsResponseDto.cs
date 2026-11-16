namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;

public sealed record PagedNotificationsResponseDto(
    IReadOnlyList<NotificationSummaryResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount);
