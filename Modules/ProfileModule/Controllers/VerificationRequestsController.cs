using System.Security.Claims;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;
using DotNetSkeleton.Modules.ProfileModule.Exceptions;
using DotNetSkeleton.Modules.ProfileModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.ProfileModule.Controllers;

[ApiController]
[Route("api/v1/verification-requests")]
public sealed class VerificationRequestsController(IVerificationRequestsService verificationRequestsService)
    : ApiControllerBase
{
    [Authorize(Policy = "permission:verification_requests:read")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedVerificationRequestsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedVerificationRequestsResponseDto>>> List(
        [FromQuery] ListVerificationRequestsRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await verificationRequestsService.ListAsync(request, cancellationToken));

    [Authorize(Policy = "permission:verification_requests:read")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<VerificationRequestDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<VerificationRequestDetailResponseDto>>> GetById(
        int id,
        CancellationToken cancellationToken) =>
        OkResponse(await verificationRequestsService.GetByIdAsync(id, cancellationToken));

    [Authorize(Policy = "permission:verification_requests:update")]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType<ApiResponse<VerificationRequestDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<VerificationRequestDetailResponseDto>>> UpdateStatus(
        int id,
        UpdateVerificationRequestStatusRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await verificationRequestsService.UpdateStatusAsync(
            id,
            GetReviewerId(),
            request,
            cancellationToken));

    private int GetReviewerId()
    {
        if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var reviewerId))
            return reviewerId;

        throw new VerificationAdminClaimsInvalidException();
    }
}
