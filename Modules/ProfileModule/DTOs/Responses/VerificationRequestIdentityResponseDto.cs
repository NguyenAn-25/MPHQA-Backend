namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestIdentityResponseDto(
    string CardId,
    string FullNameOnCard,
    string CardFrontImage,
    string CardBackImage);
