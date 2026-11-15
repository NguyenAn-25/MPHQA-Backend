namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

/// <summary>
/// Result of posting a time-based job and paying its post fee.
/// </summary>
public sealed record TimeBasedJobCreationResponseDto(
    JobDetailResponseDto Job,
    JobPaymentResponseDto Payment);

/// <summary>
/// Payment details recorded for the posted job.
/// </summary>
public sealed record JobPaymentResponseDto(
    int TransactionId,
    int Amount,
    int BalanceAfter,
    string TransactionType);
