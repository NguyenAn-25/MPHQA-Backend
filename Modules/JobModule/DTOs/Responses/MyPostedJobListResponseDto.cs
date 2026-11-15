using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>
/// A cursor-paginated page of jobs posted by the current user.
/// </summary>
public sealed record MyPostedJobListResponseDto(
    IReadOnlyList<MyPostedJobResponseDto> Items,
    string? NextCursor,
    bool HasNext);

/// <summary>
/// A job posted by the current user, including its application count.
/// </summary>
public sealed record MyPostedJobResponseDto(
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
    int ApplicantCount);
