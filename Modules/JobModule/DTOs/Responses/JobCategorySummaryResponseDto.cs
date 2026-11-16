namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record JobCategorySummaryResponseDto(
    int Id,
    string Name,
    string? Icon,
    bool IsActive,
    int? ParentId,
    IReadOnlyList<JobCategorySummaryResponseDto> Children);
