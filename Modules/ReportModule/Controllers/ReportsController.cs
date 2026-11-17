using DotNetSkeleton.Modules.ReportModule.DTOs.Requests;
using DotNetSkeleton.Modules.ReportModule.DTOs.Responses;
using DotNetSkeleton.Modules.ReportModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using DotNetSkeleton.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.ReportModule.Controllers;

[ApiController]
[Route("api/v1/reports")]
public sealed class ReportsController(IReportsService reportsService) : ApiControllerBase
{
    [Authorize(Roles = "ADMIN", Policy = "permission:reports:read")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedReportsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedReportsResponseDto>>> List(
        [FromQuery] ListReportsRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await reportsService.ListAsync(request, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:reports:read")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<ReportDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ReportDetailResponseDto>>> GetById(
        int id,
        CancellationToken cancellationToken) =>
        OkResponse(await reportsService.GetByIdAsync(id, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:reports:update")]
    [HttpPatch("{id:int}/process")]
    [ProducesResponseType<ApiResponse<ReportProcessResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ReportProcessResponseDto>>> Process(
        int id,
        ProcessReportRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await reportsService.ProcessAsync(
            id,
            User.GetRequiredUserId(),
            request,
            cancellationToken));
}
