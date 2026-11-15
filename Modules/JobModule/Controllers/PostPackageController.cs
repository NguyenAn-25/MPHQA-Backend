using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.JobModule.Controllers;

[ApiController]
[Route("api/v1/post-packages")]
public sealed class PostPackageController(IPostPackageService postPackageService) : ApiControllerBase
{
    /// <summary>
    /// Returns all post packages available for publishing a job.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<IReadOnlyList<PostPackageResponseDto>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PostPackageResponseDto>>>> GetAll(
        CancellationToken cancellationToken) =>
        OkResponse(await postPackageService.GetAllAsync(cancellationToken));
}
