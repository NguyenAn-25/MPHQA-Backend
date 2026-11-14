namespace DotNetSkeleton.Shared.Configurations;
using DotNetSkeleton.Shared.Persistence.Seeders;
using DotNetSkeleton.Modules.SkillModule.Seeders;

public static class SeederExtensions
{
    public static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        // Seed authorization reference data and shared short-term job taxonomy.
        services.AddScoped<ISeeder, AuthPermissionCatalogSeeder>();
        services.AddScoped<ISeeder, AuthDevelopmentAdminSeeder>();
        services.AddScoped<ISeeder, AuthDevelopmentAdminAccountsSeeder>();
        services.AddScoped<ISeeder, AuthDevelopmentSampleDataSeeder>();
        services.AddScoped<ISeeder, JobCategorySeeder>();
        services.AddScoped<ISeeder, SkillSeeder>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.RunAsync();
    }
}
