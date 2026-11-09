namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record PagedVerificationRequestsResponseDto(
    IReadOnlyList<VerificationRequestSummaryResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount);
