using DotNetSkeleton.Modules.ProfileModule.Enums;

namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestDetailResponseDto(
    int Id,
    string FullNameOnCard,
    string IdNumber,
    string CardFrontImage,
    string CardBackImage,
    string PortraitImage,
    VerificationRequestStatus Status,
    string? RejectionReason,
    DateTimeOffset? VerifiedAt,
    DateTimeOffset? RejectedAt,
    DateTimeOffset CreatedAt,
    VerificationRequestSenderDetailResponseDto Sender);
