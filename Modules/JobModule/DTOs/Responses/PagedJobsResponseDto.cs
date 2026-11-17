namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record PagedJobsResponseDto(
    IReadOnlyList<JobListItemResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount);
