using DotNetSkeleton.Modules.JobModule.Converters;
using DotNetSkeleton.Modules.JobModule.Services;
using DotNetSkeleton.Modules.JobModule.Services.Interfaces;
using DotNetSkeleton.Modules.JobModule.Validators;
using FluentValidation;

namespace DotNetSkeleton.Modules.JobModule.Extensions;

public static class JobServiceCollectionExtensions
{
    public static IServiceCollection AddJobModule(this IServiceCollection services)
    {
        services.AddScoped<IJobCategoriesService, JobCategoriesService>();
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JobStatusJsonConverter());
            options.SerializerOptions.Converters.Add(new PricingModelJsonConverter());
            options.SerializerOptions.Converters.Add(new ApplicationStatusJsonConverter());
        });
        services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JobStatusJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new PricingModelJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new ApplicationStatusJsonConverter());
        });

        services.AddScoped<IPostPackageService, PostPackageService>();
        services.AddScoped<IConnectionFeeSettingService, ConnectionFeeSettingService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddValidatorsFromAssemblyContaining<CreateTimeBasedJobRequestDtoValidator>();

        services.AddScoped<IAdminJobsService, AdminJobsService>();
        return services;
    }
}
