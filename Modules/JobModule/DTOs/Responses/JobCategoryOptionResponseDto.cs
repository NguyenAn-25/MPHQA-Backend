namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record JobCategoryOptionResponseDto(
    int Id,
    string Name,
    int? ParentId);
