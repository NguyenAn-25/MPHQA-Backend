using DotNetSkeleton.Modules.ProfileModule.DTOs.Requests;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.ProfileModule.Services.Interfaces;

public interface IVerificationRequestsService
{
    Task<PagedVerificationRequestsResponseDto> ListAsync(
        ListVerificationRequestsRequestDto request,
        CancellationToken cancellationToken);
    Task<PagedVerificationRequestsResponseDto> ListMineAsync(
        int senderId,
        ListVerificationRequestsRequestDto request,
        CancellationToken cancellationToken);
    Task<VerificationRequestDetailResponseDto> CreateAsync(
        int senderId,
        CreateVerificationRequestRequestDto request,
        CancellationToken cancellationToken);
    Task<VerificationRequestDetailResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<VerificationRequestDetailResponseDto> UpdateStatusAsync(
        int id,
        int reviewerId,
        UpdateVerificationRequestStatusRequestDto request,
        CancellationToken cancellationToken);
}
