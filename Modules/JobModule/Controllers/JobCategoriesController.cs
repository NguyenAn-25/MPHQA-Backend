using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.JobModule.Controllers;

[ApiController]
[Route("api/v1/job-categories")]
public sealed class JobCategoriesController(IJobCategoriesService jobCategoriesService) : ApiControllerBase
{
    [Authorize(Policy = "permission:job_categories:read")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedJobCategoriesResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedJobCategoriesResponseDto>>> List(
        [FromQuery] ListJobCategoriesRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await jobCategoriesService.ListAsync(request, cancellationToken));

    [AllowAnonymous]
    [HttpGet("options")]
    [ProducesResponseType<ApiResponse<IReadOnlyList<JobCategoryOptionResponseDto>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<JobCategoryOptionResponseDto>>>> Options(
        CancellationToken cancellationToken) =>
        OkResponse(await jobCategoriesService.GetOptionsAsync(cancellationToken));

    [Authorize(Policy = "permission:job_categories:read")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<JobCategoryDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<JobCategoryDetailResponseDto>>> GetById(
        int id,
        CancellationToken cancellationToken) =>
        OkResponse(await jobCategoriesService.GetByIdAsync(id, cancellationToken));

    [Authorize(Policy = "permission:job_categories:create")]
    [HttpPost]
    [ProducesResponseType<ApiResponse<JobCategoryDetailResponseDto>>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<JobCategoryDetailResponseDto>>> Create(
        CreateJobCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var category = await jobCategoriesService.CreateAsync(request, cancellationToken);
        return CreatedAtActionResponse(nameof(GetById), new { id = category.Id }, category);
    }

    [Authorize(Policy = "permission:job_categories:update")]
    [HttpPut("{id:int}")]
    [ProducesResponseType<ApiResponse<JobCategoryDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<JobCategoryDetailResponseDto>>> Update(
        int id,
        UpdateJobCategoryRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await jobCategoriesService.UpdateAsync(id, request, cancellationToken));

    [Authorize(Policy = "permission:job_categories:update")]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType<ApiResponse<JobCategoryDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<JobCategoryDetailResponseDto>>> UpdateStatus(
        int id,
        UpdateJobCategoryStatusRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await jobCategoriesService.UpdateStatusAsync(id, request, cancellationToken));

    [Authorize(Policy = "permission:job_categories:delete")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await jobCategoriesService.DeleteAsync(id, cancellationToken);
        return NoContentResponse();
    }
}
