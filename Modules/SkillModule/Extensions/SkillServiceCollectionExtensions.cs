using DotNetSkeleton.Modules.SkillModule.Services;
using DotNetSkeleton.Modules.SkillModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.SkillModule.Extensions;

public static class SkillServiceCollectionExtensions
{
    public static IServiceCollection AddSkillModule(this IServiceCollection services)
    {
        services.AddScoped<ISkillsService, SkillsService>();
        return services;
    }
}
