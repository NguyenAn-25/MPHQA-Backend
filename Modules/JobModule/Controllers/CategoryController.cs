using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.JobModule.Controllers;

[ApiController]
[Route("api/v1/job-categories")]
public sealed class CategoryController(ICategoryService categoryService) : ApiControllerBase
{
    /// <summary>
    /// Returns active job categories with their active child categories.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<IReadOnlyList<CategoryResponseDto>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CategoryResponseDto>>>> GetActiveCategories(
        CancellationToken cancellationToken) =>
        OkResponse(await categoryService.GetActiveCategoriesAsync(cancellationToken));
}
