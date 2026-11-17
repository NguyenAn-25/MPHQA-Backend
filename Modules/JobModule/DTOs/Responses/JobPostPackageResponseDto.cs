namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record JobPostPackageResponseDto(
    int Id,
    string Name,
    int DurationDays,
    int PriceCoin);
