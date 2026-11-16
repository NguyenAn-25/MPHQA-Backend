namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestSenderDetailResponseDto(
    int Id,
    Guid PublicId,
    string Phone,
    bool IsVerified,
    DateTimeOffset? VerifiedAt,
    VerificationRequestSenderProfileResponseDto? Profile,
    VerificationRequestIdentityResponseDto? CurrentIdentity);
