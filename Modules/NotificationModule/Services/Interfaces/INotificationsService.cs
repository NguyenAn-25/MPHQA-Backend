using DotNetSkeleton.Modules.NotificationModule.DTOs.Requests;
using DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.NotificationModule.Services.Interfaces;

public interface INotificationsService
{
    Task<PagedNotificationsResponseDto> ListAsync(
        ListNotificationsRequestDto request,
        CancellationToken cancellationToken);

    Task<NotificationDetailResponseDto> GetByIdAsync(
        int id,
        ListNotificationRecipientsRequestDto request,
        CancellationToken cancellationToken);

    Task<NotificationDetailResponseDto> CreateAsync(
        int creatorId,
        CreateNotificationRequestDto request,
        CancellationToken cancellationToken);
}
