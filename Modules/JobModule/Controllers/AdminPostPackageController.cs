using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.JobModule.Controllers;

[ApiController]
[Route("api/v1/admin/post-packages")]
[Authorize(Roles = "ADMIN")]
public sealed class AdminPostPackageController(IPostPackageService postPackageService) : ApiControllerBase
{
    /// <summary>
    /// Returns all post packages with the number of jobs using each package.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<IReadOnlyList<AdminPostPackageResponseDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AdminPostPackageResponseDto>>>> GetAll(
        CancellationToken cancellationToken) =>
        OkResponse(await postPackageService.GetAllForAdminAsync(cancellationToken));

    /// <summary>
    /// Creates a post package for publishing jobs.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<ApiResponse<AdminPostPackageResponseDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdminPostPackageResponseDto>>> Create(
        [FromBody] CreatePostPackageRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await postPackageService.CreateAsync(request, cancellationToken);
        return CreatedAtActionResponse(nameof(GetAll), null, result);
    }

    /// <summary>
    /// Updates a post package for publishing jobs.
    /// </summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType<ApiResponse<AdminPostPackageResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdminPostPackageResponseDto>>> Update(
        int id,
        [FromBody] UpdatePostPackageRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await postPackageService.UpdateAsync(id, request, cancellationToken));

    /// <summary>
    /// Deletes a post package that is not used by any job.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await postPackageService.DeleteAsync(id, cancellationToken);
        return NoContentResponse();
    }
}
