using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.NotificationModule.Enums;
using DotNetSkeleton.Shared.Pagination;

namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Requests;

public sealed class ListNotificationsRequestDto : OptionalPaginationRequestDto
{
    protected override int DefaultPageSize => 20;

    [MaxLength(200)]
    public string? Search { get; init; }

    public NotificationSendTo? SendTo { get; init; }
}
