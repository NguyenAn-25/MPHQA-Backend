using DotNetSkeleton.Modules.JobModule.DTOs.Requests;
using DotNetSkeleton.Modules.JobModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.JobModule.Services.Interfaces;

public interface IConnectionFeeSettingService
{
    /// <summary>
    /// Returns all connection fee settings ordered by effective date descending.
    /// </summary>
    Task<IReadOnlyList<ConnectionFeeSettingResponseDto>> GetAllAsync(
        CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new connection fee setting for the platform.
    /// </summary>
    Task<ConnectionFeeSettingResponseDto> CreateAsync(
        CreateConnectionFeeSettingRequestDto request,
        int adminId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Returns the connection fee currently in effect.
    /// </summary>
    Task<CurrentConnectionFeeResponseDto> GetCurrentAsync(
        CancellationToken cancellationToken);
}
