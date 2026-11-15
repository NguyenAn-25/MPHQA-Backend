using DotNetSkeleton.Modules.WalletModule.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

/// <summary>
/// Creates the development wallet fixture for the sample user.
/// </summary>
public sealed class WalletDevelopmentSeeder(
    IConfiguration configuration,
    IWebHostEnvironment environment) : ISeeder
{
    private const string SampleUserPhone = "+12025550100";
    private const int InitialBalance = 100;

    public int Order => 5;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        if (!environment.ShouldSeedDevelopmentData(configuration))
            return 0;

        var user = await context.Users
            .SingleOrDefaultAsync(candidate => candidate.Phone == SampleUserPhone);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"WalletDevelopmentSeeder requires the development sample user {SampleUserPhone}.");
        }

        if (await context.Wallets.AnyAsync(wallet => wallet.UserId == user.Id))
            return 0;

        var wallet = new WalletEntity
        {
            UserId = user.Id,
            Balance = InitialBalance
        };

        context.Wallets.Add(wallet);
        await context.SaveChangesAsync();
        return 1;
    }
}
