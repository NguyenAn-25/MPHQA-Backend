namespace DotNetSkeleton.Modules.SkillModule.DTOs.Responses;

public sealed record SkillDetailResponseDto(
    int Id,
    string Name,
    string? Description,
    string? Icon,
    bool IsActive);
