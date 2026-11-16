using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>A cursor-paginated page of applications submitted by the current user.</summary>
public sealed record MyApplicationListResponseDto(
    IReadOnlyList<MyApplicationResponseDto> Items,
    string? NextCursor,
    bool HasNext);

/// <summary>An application submitted by the current user and its job.</summary>
public sealed record MyApplicationResponseDto(
    int Id,
    int UserId,
    int JobId,
    ApplicationStatus Status,
    DateTimeOffset AppliedAt,
    string? Note,
    int? ConnectionFeeCoin,
    JobResponseDto Job);
