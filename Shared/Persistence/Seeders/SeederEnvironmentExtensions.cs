using Microsoft.AspNetCore.Hosting;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public static class SeederEnvironmentExtensions
{
    public static bool ShouldSeedDevelopmentData(
        this IWebHostEnvironment environment,
        IConfiguration configuration) =>
        environment.IsDevelopment() ||
        configuration.GetValue<bool>("Seed:EnableDevelopmentData");
}
