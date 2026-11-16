namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>
/// Represents a post package available for publishing a job.
/// </summary>
public sealed record PostPackageResponseDto(
    int Id,
    string Name,
    int DurationDays,
    int PriceCoin,
    decimal PriceMoney);
