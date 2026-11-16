namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record PagedJobCategoriesResponseDto(
    IReadOnlyList<JobCategorySummaryResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount);
