namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>
/// Represents a post package and the number of jobs using it for administration.
/// </summary>
public sealed record AdminPostPackageResponseDto(
    int Id,
    string Name,
    int DurationDays,
    int PriceCoin,
    decimal PriceMoney,
    int UsedJobCount);
