namespace DotNetSkeleton.Modules.SkillModule.DTOs.Responses;

public sealed record SkillSummaryResponseDto(
    int Id,
    string Name,
    string? Icon,
    bool IsActive);
