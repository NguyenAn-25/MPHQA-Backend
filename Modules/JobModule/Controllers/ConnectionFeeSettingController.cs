using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.JobModule.Controllers;

[ApiController]
[Route("api/v1/connection-fee-settings")]
public sealed class ConnectionFeeSettingController(
    IConnectionFeeSettingService connectionFeeSettingService) : ApiControllerBase
{
    /// <summary>
    /// Returns the connection fee currently in effect.
    /// </summary>
    [HttpGet("current")]
    [ProducesResponseType<ApiResponse<CurrentConnectionFeeResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CurrentConnectionFeeResponseDto>>> GetCurrent(
        CancellationToken cancellationToken) =>
        OkResponse(await connectionFeeSettingService.GetCurrentAsync(cancellationToken));
}
