namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record VerificationRequestIdentityComparisonResponseDto(
    string CardId,
    string FullNameOnCard,
    string CardFrontImage,
    string CardBackImage);
