using DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.ProfileModule.Services.Interfaces;

public interface IProfilesService
{
    Task<ProfileResponseDto> GetProfileAsync(int userId, CancellationToken cancellationToken);
    Task<ProfileResponseDto> UpdateProfileAsync(
        int userId,
        UpdateProfileRequestDto request,
        CancellationToken cancellationToken);
    Task<ProfileResponseDto> UpdateProfileSkillsAsync(
        int userId,
        UpdateProfileSkillsRequestDto request,
        CancellationToken cancellationToken);
    Task<ProfileResponseDto> UpdateProfileExperiencesAsync(
        int userId,
        UpdateProfileExperiencesRequestDto request,
        CancellationToken cancellationToken);
}
