using DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;
using DotNetSkeleton.Modules.ProfileModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using DotNetSkeleton.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.ProfileModule.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/profiles/me")]
public sealed class ProfilesController(IProfilesService profilesService) : ApiControllerBase
{
    [Authorize(Roles = "USER")]
    [HttpGet]
    [ProducesResponseType<ApiResponse<ProfileResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ProfileResponseDto>>> GetProfile(
        CancellationToken cancellationToken) =>
        OkResponse(await profilesService.GetProfileAsync(
            User.GetRequiredUserId(),
            cancellationToken));

    [Authorize(Roles = "USER")]
    [HttpPut]
    [ProducesResponseType<ApiResponse<ProfileResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ProfileResponseDto>>> UpdateProfile(
            UpdateProfileRequestDto request,
            CancellationToken cancellationToken) =>
            OkResponse(await profilesService.UpdateProfileAsync(
                User.GetRequiredUserId(),
                request,
                cancellationToken));

    [Authorize(Roles = "USER")]
    [HttpPut("skills")]
    [ProducesResponseType<ApiResponse<ProfileResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ProfileResponseDto>>> UpdateProfileSkills(
            UpdateProfileSkillsRequestDto request,
            CancellationToken cancellationToken) =>
            OkResponse(await profilesService.UpdateProfileSkillsAsync(
                User.GetRequiredUserId(),
                request,
                cancellationToken));

    [Authorize(Roles = "USER")]
    [HttpPut("experiences")]
    [ProducesResponseType<ApiResponse<ProfileResponseDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ProfileResponseDto>>> UpdateProfileExperiences(
            [FromBody] UpdateProfileExperiencesRequestDto request,
            CancellationToken cancellationToken) =>
            OkResponse(await profilesService.UpdateProfileExperiencesAsync(
                User.GetRequiredUserId(),
                request,
                cancellationToken));
}
