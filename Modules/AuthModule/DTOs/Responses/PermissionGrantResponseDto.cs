namespace DotNetSkeleton.Modules.AuthModule.DTOs.Responses;

public sealed record PermissionGrantResponseDto(
    PermissionFunctionResponseDto Function,
    string Action);
