using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.JobModule.Services.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryResponseDto>> GetActiveCategoriesAsync(
        CancellationToken cancellationToken);
}
