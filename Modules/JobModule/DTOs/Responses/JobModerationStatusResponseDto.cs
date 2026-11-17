using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record JobModerationStatusResponseDto(
    int Id,
    JobStatus Status,
    DateTimeOffset? AcceptedAt,
    DateTimeOffset? RejectedAt,
    DateTimeOffset? CancelledAt,
    string? RejectionReason,
    string? CancelReason);
