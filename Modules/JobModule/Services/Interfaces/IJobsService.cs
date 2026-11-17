using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.JobModule.Services.Interfaces;

public interface IJobsService
{
    Task<PagedJobsResponseDto> ListAsync(ListJobsRequestDto request, CancellationToken cancellationToken);

    Task<JobDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<JobModerationStatusResponseDto> UpdateModerationStatusAsync(
        int id,
        int adminId,
        UpdateJobModerationStatusRequestDto request,
        CancellationToken cancellationToken);
}
