namespace DotNetSkeleton.Modules.AuthModule.DTOs.Responses;

public sealed record PagedPermissionsResponseDto(
    IReadOnlyList<PermissionSummaryResponseDto> Items,
    int Page,
    int PageSize,
    int TotalCount);
