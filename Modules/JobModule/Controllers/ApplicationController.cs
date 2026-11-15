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
[Route("api/v1")]
[Authorize]
public sealed class ApplicationController(IApplicationService applicationService) : ApiControllerBase
{
    /// <summary>Returns applications submitted by the current user with their jobs.</summary>
    [HttpGet("applications/me")]
    [ProducesResponseType<ApiResponse<MyApplicationListResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<MyApplicationListResponseDto>>> ListMine(
        [FromQuery] ListJobsRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<MyApplicationListResponseDto>();

        var result = await applicationService.ListMineAsync(request, userId, cancellationToken);
        return OkResponse(result);
    }

    /// <summary>
    /// Creates an application for the current customer and the specified job.
    /// </summary>
    [HttpPost("jobs/{jobId:int}/applications")]
    [ProducesResponseType<ApiResponse<ApplicationResponseDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ApplicationResponseDto>>> Create(
        int jobId,
        CreateApplicationRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<ApplicationResponseDto>();

        var application = await applicationService.CreateAsync(
            jobId,
            userId,
            request,
            cancellationToken);

        return CreatedAtActionResponse(
            nameof(Create),
            new { jobId },
            application);
    }

    /// <summary>
    /// Accepts selected applicants for a per-connection job and charges the connection fees.
    /// </summary>
    [HttpPost("jobs/{jobId:int}/applications/accept")]
    [ProducesResponseType<ApiResponse<ApplicationAcceptanceResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ApplicationAcceptanceResponseDto>>> AcceptPerConnection(
        int jobId,
        AcceptApplicationsRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<ApplicationAcceptanceResponseDto>();

        var result = await applicationService.AcceptPerConnectionAsync(
            jobId,
            userId,
            request,
            cancellationToken);

        return OkResponse(result);
    }
}
