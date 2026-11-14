namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestSummaryResponseDto(
    int Id,
    string Status,
    DateTimeOffset SubmittedAt,
    DateTimeOffset? VerifiedAt,
    DateTimeOffset? RejectedAt,
    VerificationRequestSenderSummaryResponseDto Sender);
