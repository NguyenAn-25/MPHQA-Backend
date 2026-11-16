namespace DotNetSkeleton.Modules.AuthModule.DTOs.Responses;

public sealed record PermissionDetailResponseDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive,
    int UserCount,
    IReadOnlyList<PermissionGrantResponseDto> Details);
