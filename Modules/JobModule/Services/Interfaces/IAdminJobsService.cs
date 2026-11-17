using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.JobModule.Services.Interfaces;

public interface IAdminJobsService
{
    Task<AdminPagedJobsResponseDto> ListAsync(AdminListJobsRequestDto request, CancellationToken cancellationToken);

    Task<AdminJobDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<AdminJobModerationStatusResponseDto> UpdateModerationStatusAsync(
        int id,
        int adminId,
        AdminUpdateJobModerationStatusRequestDto request,
        CancellationToken cancellationToken);
}
