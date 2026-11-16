namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>
/// Represents the connection fee currently in effect.
/// </summary>
public sealed record CurrentConnectionFeeResponseDto(
    int Id,
    int FeeCoin,
    DateTimeOffset EffectiveFrom);
