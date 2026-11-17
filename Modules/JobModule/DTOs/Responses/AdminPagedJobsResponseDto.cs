namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record AdminPagedJobsResponseDto(
    IReadOnlyList<AdminJobListItemResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount);
