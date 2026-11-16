namespace DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;

public sealed record ProfileIdentityResponseDto(
    string CardId,
    string FullNameOnCard,
    string CardFrontImage,
    string CardBackImage);
