using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.JobModule.Services.Interfaces;

public interface IJobService
{
    Task<JobDetailResponseDto> CreatePerConnectionAsync(
        CreatePerConnectionJobRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken);

    Task<TimeBasedJobCreationResponseDto> CreateTimeBasedAsync(
        CreateTimeBasedJobRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken);

    Task<JobDetailResponseDto> UpdateAsync(
        int jobId,
        UpdateJobRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken);

    Task CancelAsync(
        int jobId,
        int currentUserId,
        CancellationToken cancellationToken);

    Task CompleteAsync(
        int jobId,
        CompleteJobRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken);

    Task<TimeBasedPaymentDetailResponseDto> GetTimeBasedPaymentAsync(
        int jobId,
        int currentUserId,
        CancellationToken cancellationToken);

    Task UnsaveForCustomerAsync(
        int id,
        int currentUserId,
        CancellationToken cancellationToken);

    Task SaveForCustomerAsync(
        int id,
        int currentUserId,
        CancellationToken cancellationToken);

    Task<JobDetailResponseDto> GetForCustomerAsync(
        int id,
        int currentUserId,
        CancellationToken cancellationToken);

    Task<JobListResponseDto> ListForCustomerAsync(
        ListJobsRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken);

    Task<MyPostedJobListResponseDto> ListMyPostedAsync(
        ListJobsRequestDto request,
        int currentUserId,
        CancellationToken cancellationToken);

    Task<SavedJobListResponseDto> ListSavedForCustomerAsync(
        int currentUserId,
        CancellationToken cancellationToken);
}
