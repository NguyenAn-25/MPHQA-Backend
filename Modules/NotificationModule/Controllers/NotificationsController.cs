using DotNetSkeleton.Modules.NotificationModule.DTOs.Requests;
using DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;
using DotNetSkeleton.Modules.NotificationModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using DotNetSkeleton.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.NotificationModule.Controllers;

[ApiController]
[Route("api/v1/notifications")]
public sealed class NotificationsController(INotificationsService notificationsService) : ApiControllerBase
{
    [Authorize(Roles = "ADMIN", Policy = "permission:notifications:read")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedNotificationsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedNotificationsResponseDto>>> List(
        [FromQuery] ListNotificationsRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await notificationsService.ListAsync(request, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:notifications:read")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<NotificationDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<NotificationDetailResponseDto>>> GetById(
        int id,
        [FromQuery] ListNotificationRecipientsRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await notificationsService.GetByIdAsync(id, request, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:notifications:create")]
    [HttpPost]
    [ProducesResponseType<ApiResponse<NotificationDetailResponseDto>>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<NotificationDetailResponseDto>>> Create(
        CreateNotificationRequestDto request,
        CancellationToken cancellationToken)
    {
        var notification = await notificationsService.CreateAsync(
            User.GetRequiredUserId(),
            request,
            cancellationToken);
        return CreatedAtActionResponse(nameof(GetById), new { id = notification.Id }, notification);
    }
}
