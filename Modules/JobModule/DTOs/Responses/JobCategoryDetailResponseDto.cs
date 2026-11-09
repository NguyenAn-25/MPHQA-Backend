namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record JobCategoryDetailResponseDto(
    int Id,
    string Name,
    string? Description,
    string? Icon,
    bool IsActive,
    int? ParentId,
    string? ParentName);
