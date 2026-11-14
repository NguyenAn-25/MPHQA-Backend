namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestDetailResponseDto(
    int Id,
    string Status,
    string FullNameOnCard,
    string IdNumber,
    string CardFrontImage,
    string CardBackImage,
    string PortraitImage,
    string? RejectionReason,
    DateTimeOffset SubmittedAt,
    DateTimeOffset? VerifiedAt,
    DateTimeOffset? RejectedAt,
    VerificationRequestSenderDetailResponseDto Sender,
    VerificationRequestProfileComparisonResponseDto? Profile);
