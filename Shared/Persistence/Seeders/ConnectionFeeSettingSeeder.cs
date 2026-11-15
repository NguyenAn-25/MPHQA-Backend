using DotNetSkeleton.Modules.AuthModule.Enums;
using DotNetSkeleton.Modules.JobModule.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class ConnectionFeeSettingSeeder(
    IConfiguration configuration,
    IWebHostEnvironment environment) : ISeeder
{
    private const int SeedFeeCoin = 10;

    public int Order => 5;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.ShouldSeedDevelopmentData(configuration))
            return 0;

        if (await context.ConnectionFeeSettings.AnyAsync(setting => setting.FeeCoin == SeedFeeCoin))
            return 0;

        var admin = await context.Users
            .Where(user => user.Role == UserRole.Admin)
            .OrderBy(user => user.Id)
            .FirstOrDefaultAsync();
        if (admin is null)
        {
            throw new InvalidOperationException(
                "ConnectionFeeSettingSeeder requires at least one administrator account.");
        }

        var now = DateTimeOffset.UtcNow;
        context.ConnectionFeeSettings.Add(new ConnectionFeeSettingEntity
        {
            FeeCoin = SeedFeeCoin,
            EffectiveFrom = now,
            CreatedAt = now,
            CreatedBy = admin.Id
        });

        await context.SaveChangesAsync();
        return 1;
    }
}
