using DotNetSkeleton.Modules.AuthModule.Entities;
using DotNetSkeleton.Modules.ProfileModule.DTOs.Responses;
using DotNetSkeleton.Modules.ProfileModule.Entities;
using Mapster;

namespace DotNetSkeleton.Modules.ProfileModule.Mappings;

public static class VerificationRequestMappingConfiguration
{
    public static TypeAdapterConfig Config { get; } = CreateConfig();

    private static TypeAdapterConfig CreateConfig()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<UserEntity, VerificationRequestSenderDetailResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.PublicId, source => source.PublicId)
            .Map(destination => destination.Phone, source => source.Phone)
            .Map(destination => destination.IsVerified, source => source.IsVerified)
            .Map(destination => destination.VerifiedAt, source => source.VerifiedAt)
            .Map(destination => destination.Profile, _ => (VerificationRequestSenderProfileResponseDto?)null)
            .Map(destination => destination.CurrentIdentity, _ => (VerificationRequestIdentityResponseDto?)null);

        config.NewConfig<VerificationRequestEntity, VerificationRequestDetailResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Status, source => source.Status)
            .Map(destination => destination.FullNameOnCard, source => source.FullNameOnCard)
            .Map(destination => destination.IdNumber, source => source.IdNumber)
            .Map(destination => destination.CardFrontImage, source => source.CardFrontImage)
            .Map(destination => destination.CardBackImage, source => source.CardBackImage)
            .Map(destination => destination.PortraitImage, source => source.PortraitImage)
            .Map(destination => destination.RejectionReason, source => source.RejectionReason)
            .Map(destination => destination.CreatedAt, source => source.CreatedAt)
            .Map(destination => destination.VerifiedAt, source => source.VerifiedAt)
            .Map(destination => destination.RejectedAt, source => source.RejectedAt)
            .Map(destination => destination.Sender, source => source.Sender);

        return config;
    }
}
