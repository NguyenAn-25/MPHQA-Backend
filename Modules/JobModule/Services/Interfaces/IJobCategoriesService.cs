using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.JobModule.Services.Interfaces;

public interface IJobCategoriesService
{
    Task<PagedJobCategoriesResponseDto> ListAsync(ListJobCategoriesRequestDto request, CancellationToken cancellationToken);
    Task<IReadOnlyList<JobCategoryOptionResponseDto>> GetOptionsAsync(CancellationToken cancellationToken);
    Task<JobCategoryDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<JobCategoryDetailResponseDto> CreateAsync(CreateJobCategoryRequestDto request, CancellationToken cancellationToken);
    Task<JobCategoryDetailResponseDto> UpdateAsync(int id, UpdateJobCategoryRequestDto request, CancellationToken cancellationToken);
    Task<JobCategoryDetailResponseDto> UpdateStatusAsync(int id, UpdateJobCategoryStatusRequestDto request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
