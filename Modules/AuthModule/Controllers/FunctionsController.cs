using DotNetSkeleton.Modules.AuthModule.DTOs.Responses;
using DotNetSkeleton.Modules.AuthModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.AuthModule.Controllers;

[ApiController]
[Route("api/v1/functions")]
public sealed class FunctionsController(IFunctionsService functionsService) : ApiControllerBase
{
    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<IReadOnlyList<FunctionSummaryResponseDto>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<FunctionSummaryResponseDto>>>> List(
        CancellationToken cancellationToken) =>
        OkResponse(await functionsService.ListAsync(cancellationToken));
}
