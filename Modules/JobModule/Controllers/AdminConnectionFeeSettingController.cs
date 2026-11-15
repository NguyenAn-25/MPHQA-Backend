using System.Security.Claims;
using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.JobModule.Controllers;

[ApiController]
[Route("api/v1/admin/connection-fee-settings")]
[Authorize(Roles = "ADMIN")]
public sealed class AdminConnectionFeeSettingController(
    IConnectionFeeSettingService connectionFeeSettingService) : ApiControllerBase
{
    /// <summary>
    /// Returns all connection fee settings with creator details and current active status.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<IReadOnlyList<ConnectionFeeSettingResponseDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ConnectionFeeSettingResponseDto>>>> GetAll(
        CancellationToken cancellationToken) =>
        OkResponse(await connectionFeeSettingService.GetAllAsync(cancellationToken));

    /// <summary>
    /// Creates a new connection fee setting.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<ApiResponse<ConnectionFeeSettingResponseDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ConnectionFeeSettingResponseDto>>> Create(
        [FromBody] CreateConnectionFeeSettingRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<ConnectionFeeSettingResponseDto>();

        var result = await connectionFeeSettingService.CreateAsync(request, userId, cancellationToken);
        return CreatedAtActionResponse(nameof(GetAll), null, result);
    }
}
