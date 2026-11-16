namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

using DotNetSkeleton.Modules.JobModule.Enums;

/// <summary>
/// A cursor-paginated page of jobs.
/// </summary>
public sealed record JobListResponseDto(
    IReadOnlyList<JobResponseDto> Items,
    string? NextCursor,
    bool HasNext);

/// <summary>
/// Job details shown in the customer job feed.
/// </summary>
public sealed record JobResponseDto(
    int Id,
    decimal SalaryValue,
    string SalaryUnit,
    decimal TotalSalaryPayment,
    decimal TotalPlatformFee,
    decimal TotalExpense,
    DateTimeOffset? CompletedAt,
    DateTimeOffset? WorkingAt,
    DateTimeOffset? FullAt,
    DateTimeOffset? RejectedAt,
    DateTimeOffset? AcceptedAt,
    bool IsPaid,
    string Title,
    string Description,
    string Address,
    decimal? Longitude,
    decimal? Latitude,
    DateTimeOffset? CancelledAt,
    DateTimeOffset CreatedAt,
    int RequiredNumberOfWorkers,
    DateTimeOffset? ExpiredAt,
    DateTimeOffset TimeStartAt,
    DateTimeOffset TimeEndAt,
    JobStatus Status,
    string? RequiredDescription,
    string? AutoReplyMessage,
    PricingModel PricingModel,
    int PostFeeCoin,
    int CreatedBy,
    JobPosterResponseDto Poster,
    JobCategoryResponseDto Category,
    string? FirstImage,
    bool IsSaved,
    decimal? AverageRating,
    int TotalRatings);

/// <summary>
/// A saved job with the current customer's application status.
/// </summary>
public sealed record SavedJobResponseDto(
    int Id,
    decimal SalaryValue,
    string SalaryUnit,
    decimal TotalSalaryPayment,
    decimal TotalPlatformFee,
    decimal TotalExpense,
    DateTimeOffset? CompletedAt,
    DateTimeOffset? WorkingAt,
    DateTimeOffset? FullAt,
    DateTimeOffset? RejectedAt,
    DateTimeOffset? AcceptedAt,
    bool IsPaid,
    string Title,
    string Description,
    string Address,
    decimal? Longitude,
    decimal? Latitude,
    DateTimeOffset? CancelledAt,
    DateTimeOffset CreatedAt,
    int RequiredNumberOfWorkers,
    DateTimeOffset? ExpiredAt,
    DateTimeOffset TimeStartAt,
    DateTimeOffset TimeEndAt,
    JobStatus Status,
    string? RequiredDescription,
    string? AutoReplyMessage,
    PricingModel PricingModel,
    int PostFeeCoin,
    int CreatedBy,
    JobPosterResponseDto Poster,
    JobCategoryResponseDto Category,
    string? FirstImage,
    bool IsSaved,
    decimal? AverageRating,
    int TotalRatings,
    ApplicationStatus? ApplicationStatus);

/// <summary>
/// All saved jobs for the current customer.
/// </summary>
public sealed record SavedJobListResponseDto(
    IReadOnlyList<SavedJobResponseDto> Items);

/// <summary>
/// Full job details shown to a customer.
/// </summary>
public sealed record JobDetailResponseDto(
    int Id,
    decimal SalaryValue,
    string SalaryUnit,
    decimal TotalSalaryPayment,
    decimal TotalPlatformFee,
    decimal TotalExpense,
    DateTimeOffset? CompletedAt,
    DateTimeOffset? WorkingAt,
    DateTimeOffset? FullAt,
    DateTimeOffset? RejectedAt,
    DateTimeOffset? AcceptedAt,
    bool IsPaid,
    string Title,
    string Description,
    string Address,
    decimal? Longitude,
    decimal? Latitude,
    DateTimeOffset? CancelledAt,
    DateTimeOffset CreatedAt,
    int RequiredNumberOfWorkers,
    DateTimeOffset? ExpiredAt,
    DateTimeOffset TimeStartAt,
    DateTimeOffset TimeEndAt,
    JobStatus Status,
    string? RequiredDescription,
    string? AutoReplyMessage,
    PricingModel PricingModel,
    int PostFeeCoin,
    int CreatedBy,
    JobPosterResponseDto Poster,
    JobCategoryResponseDto Category,
    IReadOnlyList<string> Images,
    bool IsSaved,
    decimal? AverageRating,
    int TotalRatings);

/// <summary>
/// Public information about the user who posted a job.
/// </summary>
public sealed record JobPosterResponseDto(
    int Id,
    Guid PublicId,
    string FullName,
    string? Avatar,
    bool IsVerified);

/// <summary>
/// Category information attached to a job.
/// </summary>
public sealed record JobCategoryResponseDto(
    int Id,
    string Name,
    string? Description,
    string? Icon);
