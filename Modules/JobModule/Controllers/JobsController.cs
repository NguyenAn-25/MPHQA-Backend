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
[Route("api/v1/jobs")]
public sealed class JobsController(IJobsService jobsService) : ApiControllerBase
{
    [Authorize(Roles = "ADMIN", Policy = "permission:jobs:read")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedJobsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedJobsResponseDto>>> List(
        [FromQuery] ListJobsRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await jobsService.ListAsync(request, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:jobs:read")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<JobDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<JobDetailResponseDto>>> GetById(
        int id,
        CancellationToken cancellationToken) =>
        OkResponse(await jobsService.GetByIdAsync(id, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:jobs:update")]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType<ApiResponse<JobModerationStatusResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<JobModerationStatusResponseDto>>> UpdateStatus(
        int id,
        UpdateJobModerationStatusRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await jobsService.UpdateModerationStatusAsync(
            id,
            User.GetRequiredUserId(),
            request,
            cancellationToken));
}
