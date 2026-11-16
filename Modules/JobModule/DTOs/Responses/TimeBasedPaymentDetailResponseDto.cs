using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>
/// Details of the post-fee payment for a time-based job.
/// </summary>
public sealed record TimeBasedPaymentDetailResponseDto(
    int JobId,
    string JobTitle,
    PricingModel PricingModel,
    int PostPackageId,
    string PostPackageName,
    int PostPackageDurationDays,
    int PostFeeCoin,
    int TransactionId,
    string TransactionType,
    int PaidAmount,
    int BalanceAfter,
    DateTimeOffset PaidAt);
