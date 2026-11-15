using System.ComponentModel.DataAnnotations;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Requests;

/// <summary>
/// Payload for creating a new connection fee setting.
/// </summary>
public sealed class CreateConnectionFeeSettingRequestDto
{
    /// <summary>
    /// Connection fee in coins.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Mức phí kết nối phải là số nguyên không âm.")]
    public int FeeCoin { get; init; }

    /// <summary>
    /// Effective start time for this fee setting. If omitted or null, applies immediately from current time.
    /// </summary>
    public DateTimeOffset? EffectiveFrom { get; init; }
}
