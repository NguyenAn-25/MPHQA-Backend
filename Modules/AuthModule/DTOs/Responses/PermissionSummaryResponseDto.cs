namespace DotNetSkeleton.Modules.AuthModule.DTOs.Responses;

public sealed record PermissionSummaryResponseDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive,
    int UserCount);
