using DotNetSkeleton.Modules.SkillModule.DTOs.Responses;
using DotNetSkeleton.Modules.SkillModule.Entities;
using Mapster;

namespace DotNetSkeleton.Modules.SkillModule.Mappings;

public static class SkillMappingConfiguration
{
    public static TypeAdapterConfig Config { get; } = CreateConfig();

    private static TypeAdapterConfig CreateConfig()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<SkillEntity, SkillSummaryResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Name, source => source.Name)
            .Map(destination => destination.Icon, source => source.Icon)
            .Map(destination => destination.IsActive, source => source.IsActive);

        config.NewConfig<SkillEntity, SkillDetailResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Name, source => source.Name)
            .Map(destination => destination.Description, source => source.Description)
            .Map(destination => destination.Icon, source => source.Icon)
            .Map(destination => destination.IsActive, source => source.IsActive);

        config.NewConfig<SkillEntity, SkillOptionResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Name, source => source.Name);

        return config;
    }
}
