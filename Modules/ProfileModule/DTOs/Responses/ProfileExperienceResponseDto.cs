using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record ProfileExperienceResponseDto(
    int Id,
    string JobTitle,
    string CompanyName,
    string? Description,
    DateTimeOffset CreatedAt,
    string? Title,
    JobCategoryDetailResponseDto? JobCategory);
