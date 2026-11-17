namespace DotNetSkeleton.Shared.Configurations;

using DotNetSkeleton.Shared.Persistence.Seeders;
using DotNetSkeleton.Modules.SkillModule.Seeders;
using DotNetSkeleton.Modules.ProfileModule.Seeders;
using DotNetSkeleton.Modules.NotificationModule.Seeders;
using DotNetSkeleton.Modules.ReportModule.Seeders;

public static class SeederExtensions
{
    public static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        // Seed authorization reference data and shared short-term job taxonomy.
        services.AddScoped<ISeeder, AuthPermissionCatalogSeeder>();
        services.AddScoped<ISeeder, JobCategorySeeder>();
        services.AddScoped<ISeeder, PaymentMethodSeeder>();
        services.AddScoped<ISeeder, AuthDevelopmentAdminSeeder>();
        services.AddScoped<ISeeder, AuthDevelopmentAdminAccountsSeeder>();
        services.AddScoped<ISeeder, AuthDevelopmentSampleDataSeeder>();
        services.AddScoped<ISeeder, SkillSeeder>();
        services.AddScoped<ISeeder, ProfileDevelopmentSampleDataSeeder>();
        services.AddScoped<ISeeder, NotificationDevelopmentSampleDataSeeder>();
        services.AddScoped<ISeeder, WalletDevelopmentSeeder>();
        services.AddScoped<ISeeder, ConnectionFeeSettingSeeder>();
        services.AddScoped<ISeeder, PostPackageSeeder>();
        services.AddScoped<ISeeder, JobSeeder>();
        services.AddScoped<ISeeder, JobImageSeeder>();
        services.AddScoped<ISeeder, SavedJobSeeder>();
        services.AddScoped<ISeeder, ReportDevelopmentSampleDataSeeder>();
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
