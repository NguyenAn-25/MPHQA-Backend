using DotNetSkeleton.Modules.SkillModule.DTOs.Requests;
using DotNetSkeleton.Modules.SkillModule.DTOs.Responses;
using DotNetSkeleton.Modules.SkillModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.SkillModule.Controllers;

[ApiController]
[Route("api/v1/skills")]
public sealed class SkillsController(ISkillsService skillsService) : ApiControllerBase
{
    [Authorize(Policy = "permission:skills:read")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedSkillsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedSkillsResponseDto>>> List(
        [FromQuery] ListSkillsRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await skillsService.ListAsync(request, cancellationToken));

    [AllowAnonymous]
    [HttpGet("options")]
    [ProducesResponseType<ApiResponse<IReadOnlyList<SkillOptionResponseDto>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SkillOptionResponseDto>>>> Options(
        CancellationToken cancellationToken) =>
        OkResponse(await skillsService.GetOptionsAsync(cancellationToken));

    [Authorize(Policy = "permission:skills:read")]
    [HttpGet("{id:int}")]
    [ProducesResponseType<ApiResponse<SkillDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SkillDetailResponseDto>>> GetById(
        int id,
        CancellationToken cancellationToken) =>
        OkResponse(await skillsService.GetByIdAsync(id, cancellationToken));

    [Authorize(Policy = "permission:skills:create")]
    [HttpPost]
    [ProducesResponseType<ApiResponse<SkillDetailResponseDto>>(StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<SkillDetailResponseDto>>> Create(
        CreateSkillRequestDto request,
        CancellationToken cancellationToken)
    {
        var skill = await skillsService.CreateAsync(request, cancellationToken);
        return CreatedAtActionResponse(nameof(GetById), new { id = skill.Id }, skill);
    }

    [Authorize(Policy = "permission:skills:update")]
    [HttpPut("{id:int}")]
    [ProducesResponseType<ApiResponse<SkillDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SkillDetailResponseDto>>> Update(
        int id,
        UpdateSkillRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await skillsService.UpdateAsync(id, request, cancellationToken));

    [Authorize(Policy = "permission:skills:update")]
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType<ApiResponse<SkillDetailResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SkillDetailResponseDto>>> UpdateStatus(
        int id,
        UpdateSkillStatusRequestDto request,
        CancellationToken cancellationToken) =>
        OkResponse(await skillsService.UpdateStatusAsync(id, request, cancellationToken));

    [Authorize(Policy = "permission:skills:delete")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await skillsService.DeleteAsync(id, cancellationToken);
        return NoContentResponse();
    }
}
