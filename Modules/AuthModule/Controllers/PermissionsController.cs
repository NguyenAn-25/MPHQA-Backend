using DotNetSkeleton.Modules.AuthModule.DTOs.Requests;
using DotNetSkeleton.Modules.AuthModule.DTOs.Responses;
using DotNetSkeleton.Modules.AuthModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.AuthModule.Controllers;

[ApiController]
[Route("api/v1/permissions")]
public sealed class PermissionsController(IPermissionsService permissionsService) : ApiControllerBase
{
    [Authorize(Roles = "ADMIN", Policy = "permission:permissions:read")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedPermissionsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedPermissionsResponseDto>>> List(
        [FromQuery] ListPermissionsRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await permissionsService.ListAsync(request, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:permissions:read")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<PermissionDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PermissionDetailResponseDto>>> GetById(
        int id,
        CancellationToken cancellationToken) =>
        OkResponse(await permissionsService.GetByIdAsync(id, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:permissions:create")]
    [HttpPost]
    [ProducesResponseType<ApiResponse<PermissionDetailResponseDto>>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<PermissionDetailResponseDto>>> Create(
        CreatePermissionRequestDto request,
        CancellationToken cancellationToken)
    {
        var profile = await permissionsService.CreateAsync(request, cancellationToken);
        return CreatedAtActionResponse(nameof(GetById), new { id = profile.Id }, profile);
    }

    [Authorize(Roles = "ADMIN", Policy = "permission:permissions:update")]
    [HttpPut("{id:int}")]
    [ProducesResponseType<ApiResponse<PermissionDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PermissionDetailResponseDto>>> Update(
        int id,
        UpdatePermissionRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await permissionsService.UpdateAsync(id, request, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:permissions:update")]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType<ApiResponse<PermissionDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PermissionDetailResponseDto>>> UpdateStatus(
        int id,
        UpdatePermissionStatusRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await permissionsService.UpdateStatusAsync(id, request, cancellationToken));

    [Authorize(Roles = "ADMIN", Policy = "permission:permissions:delete")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await permissionsService.DeleteAsync(id, cancellationToken);
        return NoContentResponse();
    }
}
