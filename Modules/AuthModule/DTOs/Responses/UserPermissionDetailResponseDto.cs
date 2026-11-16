namespace DotNetSkeleton.Modules.AuthModule.DTOs.Responses;

public sealed record UserPermissionDetailResponseDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive);
