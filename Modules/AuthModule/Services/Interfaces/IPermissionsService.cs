using DotNetSkeleton.Modules.AuthModule.DTOs.Requests;
using DotNetSkeleton.Modules.AuthModule.DTOs.Responses;
using DotNetSkeleton.Modules.AuthModule.Enums;

namespace DotNetSkeleton.Modules.AuthModule.Services.Interfaces;

public interface IPermissionsService
{
    Task<PagedPermissionsResponseDto> ListAsync(
        ListPermissionsRequestDto request,
        CancellationToken cancellationToken);

    Task<PermissionDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<PermissionDetailResponseDto> CreateAsync(
        CreatePermissionRequestDto request,
        CancellationToken cancellationToken);

    Task<PermissionDetailResponseDto> UpdateAsync(
        int id,
        UpdatePermissionRequestDto request,
        CancellationToken cancellationToken);

    Task<PermissionDetailResponseDto> UpdateStatusAsync(
        int id,
        UpdatePermissionStatusRequestDto request,
        CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
