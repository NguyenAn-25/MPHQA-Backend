namespace DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

public sealed record PagedReportsResponseDto(
    IReadOnlyList<ReportSummaryResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount);
