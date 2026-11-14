using DotNetSkeleton.Modules.JobModule.DTOs.Responses;
using DotNetSkeleton.Modules.JobModule.Entities;
using Mapster;

namespace DotNetSkeleton.Modules.JobModule.Mappings;

public static class JobCategoryMappingConfiguration
{
    public static TypeAdapterConfig Config { get; } = CreateConfig();

    private static TypeAdapterConfig CreateConfig()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<JobCategoryEntity, JobCategorySummaryResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Name, source => source.Name)
            .Map(destination => destination.Icon, source => source.Icon)
            .Map(destination => destination.IsActive, source => source.IsActive)
            .Map(destination => destination.ParentId, source => source.ParentId)
            .Map(destination => destination.Children, _ => Array.Empty<JobCategorySummaryResponseDto>());

        config.NewConfig<JobCategoryEntity, JobCategoryDetailResponseDto>()
            .Map(destination => destination.Id, source => source.Id)
            .Map(destination => destination.Name, source => source.Name)
            .Map(destination => destination.Description, source => source.Description)
            .Map(destination => destination.Icon, source => source.Icon)
            .Map(destination => destination.IsActive, source => source.IsActive)
            .Map(destination => destination.ParentId, source => source.ParentId)
            .Map(destination => destination.ParentName, _ => (string?)null);

        return config;
    }
}
