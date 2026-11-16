using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record JobApplicantResponseDto(
    int Id,
    int UserId,
    int JobId,
    ApplicationStatus Status,
    DateTimeOffset AppliedAt,
    string? Note,
    int? ConnectionFeeCoin,
    string Phone,
    ApplicantProfileResponseDto? Profile);

public sealed record ApplicantProfileResponseDto(
    string Fullname,
    string? Major,
    string? Gender,
    string? Avatar,
    DateOnly? Birthdate,
    string? Address,
    string? Note);
