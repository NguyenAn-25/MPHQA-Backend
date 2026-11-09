namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestSenderDetailResponseDto(
    int Id,
    Guid PublicId,
    string Phone,
    bool IsActive,
    bool IsPhoneVerified,
    DateTimeOffset? IdentityVerifiedAt,
    DateTimeOffset? IdentityRejectedAt);
