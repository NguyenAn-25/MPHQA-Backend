using DotNetSkeleton.Modules.JobModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class PostPackageSeeder : ISeeder
{
    private static readonly PostPackageDefinition[] Packages =
    [
        new("Gói hiển thị tin 3 ngày", 3, 25, 25000m),
        new("Gói hiển thị tin 7 ngày", 7, 46, 46000m),
        new("Gói hiển thị tin 7 ngày tăng cường hiển thị", 7, 65, 65000m),
        new("Gói hiển thị tin 14 ngày", 14, 77, 77000m),
        new("Gói hiển thị tin 14 ngày tăng cường hiển thị", 14, 106, 106000m),
        new("Gói hiển thị tin 30 ngày", 30, 130, 130000m),
        new("Gói hiển thị tin 30 ngày", 30, 196, 196000m)
    ];

    public int Order => 2;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        var existingPackages = await context.PostPackages
            .Select(package => new
            {
                package.Name,
                package.DurationDays,
                package.PriceCoin,
                package.PriceMoney
            })
            .ToListAsync();

        var existingKeys = existingPackages
            .Select(package => (package.Name, package.DurationDays, package.PriceCoin, package.PriceMoney))
            .ToHashSet();

        var added = 0;
        foreach (var package in Packages)
        {
            var key = (package.Name, package.DurationDays, package.PriceCoin, package.PriceMoney);
            if (!existingKeys.Add(key))
                continue;

            context.PostPackages.Add(new PostPackageEntity
            {
                Name = package.Name,
                DurationDays = package.DurationDays,
                PriceCoin = package.PriceCoin,
                PriceMoney = package.PriceMoney
            });
            added++;
        }

        if (added > 0)
            await context.SaveChangesAsync();

        return added;
    }

    private sealed record PostPackageDefinition(
        string Name,
        int DurationDays,
        int PriceCoin,
        decimal PriceMoney);
}
