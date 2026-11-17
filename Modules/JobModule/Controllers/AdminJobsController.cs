using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using DotNetSkeleton.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.JobModule.Controllers;

[ApiController]
[Route("api/v1/admin/jobs")]
public sealed class AdminJobsController(IAdminJobsService adminJobsService) : ApiControllerBase
{
    [Authorize(Roles = "ADMIN", Policy = "permission:jobs:read")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<AdminPagedJobsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AdminPagedJobsResponseDto>>> List(
        [FromQuery] AdminListJobsRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await adminJobsService.ListAsync(request, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:jobs:read")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<AdminJobDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AdminJobDetailResponseDto>>> GetById(
        int id,
        CancellationToken cancellationToken) =>
        OkResponse(await adminJobsService.GetByIdAsync(id, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:jobs:update")]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType<ApiResponse<AdminJobModerationStatusResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AdminJobModerationStatusResponseDto>>> UpdateStatus(
        int id,
        AdminUpdateJobModerationStatusRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await adminJobsService.UpdateModerationStatusAsync(
            id,
            User.GetRequiredUserId(),
            request,
            cancellationToken));
}
