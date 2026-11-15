using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.JobModule.Services.Interfaces;

public interface IApplicationService
{
    Task<ApplicationResponseDto> CreateAsync(
        int jobId,
        int currentUserId,
        CreateApplicationRequestDto request,
        CancellationToken cancellationToken);

    Task<ApplicationAcceptanceResponseDto> AcceptPerConnectionAsync(
        int jobId,
        int currentUserId,
        AcceptApplicationsRequestDto request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<JobApplicantResponseDto>> ListApplicantsAsync(
        int jobId,
        int currentUserId,
        CancellationToken cancellationToken);

    Task<MyApplicationListResponseDto> ListMineAsync(
        ListJobsRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken);
}
