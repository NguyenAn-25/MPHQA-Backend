namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record ProfileResponseDto(
    int Id,
    string Fullname,
    string? Note,
    string? Major,
    string? Gender,
    string? Avatar,
    DateOnly? Birthdate,
    string? Address,
    decimal? Longitude,
    decimal? Latitude,
    ProfileIdentityResponseDto? Identity,
    IReadOnlyList<ProfileExperienceResponseDto> Experiences,
    IReadOnlyList<ProfileSkillResponseDto> Skills);
