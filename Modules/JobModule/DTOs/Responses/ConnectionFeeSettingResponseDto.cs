namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>
/// Information about the administrator who created the connection fee setting.
/// </summary>
public sealed record ConnectionFeeSettingCreatorResponseDto(
    int Id,
    Guid PublicId,
    string Phone,
    string? FullName);

/// <summary>
/// A connection fee setting record for administration.
/// </summary>
public sealed record ConnectionFeeSettingResponseDto(
    int Id,
    int FeeCoin,
    DateTimeOffset EffectiveFrom,
    DateTimeOffset CreatedAt,
    int CreatedBy,
    ConnectionFeeSettingCreatorResponseDto? Creator,
    bool IsActive);
