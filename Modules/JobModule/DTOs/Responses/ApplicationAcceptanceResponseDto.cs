namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record ApplicationAcceptanceResponseDto(
    int JobId,
    IReadOnlyList<ApplicationPaymentDetailResponseDto> Payments,
    int TotalAmount,
    int BalanceAfter);

public sealed record ApplicationPaymentDetailResponseDto(
    int ApplicationId,
    int ApplicantUserId,
    int TransactionId,
    int Amount,
    int BalanceAfter,
    string TransactionType);
