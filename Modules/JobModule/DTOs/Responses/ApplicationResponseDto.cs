namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

using DotNetSkeleton.Modules.JobModule.Enums;

/// <summary>
/// An application submitted by a customer for a job.
/// </summary>
public sealed record ApplicationResponseDto(
    int Id,
    int UserId,
    int JobId,
    ApplicationStatus Status,
    DateTimeOffset AppliedAt,
    string? Note,
    int? ConnectionFeeCoin);
