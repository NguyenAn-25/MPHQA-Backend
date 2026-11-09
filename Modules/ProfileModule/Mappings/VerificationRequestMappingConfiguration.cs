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

        config.NewConfig<UserEntity, VerificationRequestSenderSummaryResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.PublicId, source => source.PublicId)
            .Map(destination => destination.Phone, source => source.Phone);

        config.NewConfig<VerificationRequestEntity, VerificationRequestSummaryResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Status, source => source.Status)
            .Map(destination => destination.SubmittedAt, source => source.CreatedAt)
            .Map(destination => destination.VerifiedAt, source => source.VerifiedAt)
            .Map(destination => destination.RejectedAt, source => source.RejectedAt)
            .Map(destination => destination.Sender, source => source.Sender);

        config.NewConfig<UserEntity, VerificationRequestSenderDetailResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.PublicId, source => source.PublicId)
            .Map(destination => destination.Phone, source => source.Phone)
            .Map(destination => destination.IsActive, source => source.IsActive)
            .Map(destination => destination.IsPhoneVerified, source => source.IsVerified)
            .Map(destination => destination.IdentityVerifiedAt, source => source.IdentityVerifiedAt)
            .Map(destination => destination.IdentityRejectedAt, source => source.IdentityRejectedAt);

        config.NewConfig<VerificationRequestEntity, VerificationRequestDetailResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Status, source => source.Status)
            .Map(destination => destination.FullNameOnCard, source => source.FullNameOnCard)
            .Map(destination => destination.IdNumber, source => source.IdNumber)
            .Map(destination => destination.CardFrontImage, source => source.CardFrontImage)
            .Map(destination => destination.CardBackImage, source => source.CardBackImage)
            .Map(destination => destination.PortraitImage, source => source.PortraitImage)
            .Map(destination => destination.RejectionReason, source => source.RejectionReason)
            .Map(destination => destination.SubmittedAt, source => source.CreatedAt)
            .Map(destination => destination.VerifiedAt, source => source.VerifiedAt)
            .Map(destination => destination.RejectedAt, source => source.RejectedAt)
            .Map(destination => destination.Sender, source => source.Sender)
            .Map(destination => destination.Profile, _ => (VerificationRequestProfileComparisonResponseDto?)null);

        return config;
    }
}
