namespace DotNetSkeleton.Modules.AuthModule.DTOs.Responses;

public sealed record PermissionFunctionResponseDto(
    int Id,
    string Code,
    string NameVi,
    string NameEn);
