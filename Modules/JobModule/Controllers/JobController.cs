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
[Route("api/v1/jobs")]
[Authorize]
public sealed class JobController(IJobService jobService, IApplicationService applicationService) : ApiControllerBase
{
    /// <summary>
    /// Returns all applications for a job owned by the current user, with applicant profile details.
    /// </summary>
    [HttpGet("{id:int}/applicants")]
    [ProducesResponseType<ApiResponse<IReadOnlyList<JobApplicantResponseDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<JobApplicantResponseDto>>>> ListApplicants(
        int id,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<IReadOnlyList<JobApplicantResponseDto>>();

        var result = await applicationService.ListApplicantsAsync(id, userId, cancellationToken);
        return OkResponse(result);
    }

    /// <summary>
    /// Posts a new per-connection job without charging a payment at this step.
    /// </summary>
    [HttpPost("per-connection")]
    [ProducesResponseType<ApiResponse<JobDetailResponseDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<JobDetailResponseDto>>> CreatePerConnection(
        [FromBody] CreatePerConnectionJobRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<JobDetailResponseDto>();

        var result = await jobService.CreatePerConnectionAsync(request, userId, cancellationToken);
        return CreatedAtActionResponse(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Posts a new time-based job in the pending state.
    /// </summary>
    [HttpPost("time-based")]
    [ProducesResponseType<ApiResponse<TimeBasedJobCreationResponseDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<TimeBasedJobCreationResponseDto>>> CreateTimeBased(
        [FromBody] CreateTimeBasedJobRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<TimeBasedJobCreationResponseDto>();

        var result = await jobService.CreateTimeBasedAsync(request, userId, cancellationToken);
        return CreatedAtActionResponse(nameof(GetById), new { id = result.Job.Id }, result);
    }

    /// <summary>
    /// Updates the current user's job while it is still pending.
    /// </summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType<ApiResponse<JobDetailResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<JobDetailResponseDto>>> Update(
        int id,
        [FromBody] UpdateJobRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<JobDetailResponseDto>();

        var result = await jobService.UpdateAsync(id, request, userId, cancellationToken);
        return OkResponse(result);
    }

    /// <summary>
    /// Confirms that a working job will not take place and cancels all of its applications.
    /// </summary>
    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(
        int id,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse();

        await jobService.CancelAsync(id, userId, cancellationToken);
        return NoContentResponse();
    }

    /// <summary>
    /// Completes a working job and marks selected applicants as finished and the rest as no-show.
    /// </summary>
    [HttpPost("{id:int}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Complete(
        int id,
        [FromBody] CompleteJobRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse();

        await jobService.CompleteAsync(id, request, userId, cancellationToken);
        return NoContentResponse();
    }

    /// <summary>
    /// Returns the post-fee payment details for a time-based job owned by the current user.
    /// </summary>
    [HttpGet("{id:int}/post-fee-payment")]
    [ProducesResponseType<ApiResponse<TimeBasedPaymentDetailResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TimeBasedPaymentDetailResponseDto>>> GetTimeBasedPayment(
        int id,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<TimeBasedPaymentDetailResponseDto>();

        var result = await jobService.GetTimeBasedPaymentAsync(id, userId, cancellationToken);
        return OkResponse(result);
    }

    /// <summary>
    /// Returns a cursor-paginated customer job feed with filters and rating information.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<JobListResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<JobListResponseDto>>> List(
        [FromQuery] ListJobsRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<JobListResponseDto>();

        var result = await jobService.ListForCustomerAsync(request, userId, cancellationToken);
        return OkResponse(result);
    }

    /// <summary>
    /// Returns the current user's posted jobs with the number of applications for each job.
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType<ApiResponse<MyPostedJobListResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<MyPostedJobListResponseDto>>> ListMyPosted(
        [FromQuery] ListJobsRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<MyPostedJobListResponseDto>();

        var result = await jobService.ListMyPostedAsync(request, userId, cancellationToken);
        return OkResponse(result);
    }

    /// <summary>
    /// Returns the current customer's saved jobs with application status when available.
    /// </summary>
    [HttpGet("saved")]
    [ProducesResponseType<ApiResponse<SavedJobListResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SavedJobListResponseDto>>> ListSaved(
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<SavedJobListResponseDto>();

        var result = await jobService.ListSavedForCustomerAsync(userId, cancellationToken);
        return OkResponse(result);
    }

    /// <summary>
    /// Returns full details of a job for the current customer.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<JobDetailResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<JobDetailResponseDto>>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse<JobDetailResponseDto>();

        var result = await jobService.GetForCustomerAsync(id, userId, cancellationToken);
        return OkResponse(result);
    }

    /// <summary>
    /// Saves a job for the current customer.
    /// </summary>
    [HttpPost("{id:int}/save")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Save(
        int id,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse();

        await jobService.SaveForCustomerAsync(id, userId, cancellationToken);
        return NoContentResponse();
    }

    /// <summary>
    /// Removes a saved job for the current customer.
    /// </summary>
    [HttpDelete("{id:int}/save")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unsave(
        int id,
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return UnauthorizedResponse();

        await jobService.UnsaveForCustomerAsync(id, userId, cancellationToken);
        return NoContentResponse();
    }
}
