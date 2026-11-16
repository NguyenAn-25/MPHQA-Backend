using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.JobModule.Services.Interfaces;

public interface IPostPackageService
{
    Task<IReadOnlyList<PostPackageResponseDto>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AdminPostPackageResponseDto>> GetAllForAdminAsync(
        CancellationToken cancellationToken);

    Task<AdminPostPackageResponseDto> CreateAsync(
        CreatePostPackageRequestDto request,
        CancellationToken cancellationToken);

    Task<AdminPostPackageResponseDto> UpdateAsync(
        int id,
        UpdatePostPackageRequestDto request,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken);
}
