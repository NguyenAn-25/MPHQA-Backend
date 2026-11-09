using DotNetSkeleton.Modules.JobModule.Services;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;

namespace DotNetSkeleton.Modules.JobModule.Extensions;

public static class JobServiceCollectionExtensions
{
    public static IServiceCollection AddJobModule(this IServiceCollection services)
    {
        services.AddScoped<IJobCategoriesService, JobCategoriesService>();
        return services;
    }
}
