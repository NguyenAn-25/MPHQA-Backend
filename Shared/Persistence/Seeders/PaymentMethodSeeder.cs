using DotNetSkeleton.Modules.WalletModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Shared.Persistence.Seeders;

public sealed class PaymentMethodSeeder : ISeeder
{
    private static readonly PaymentMethodDefinition[] PaymentMethods =
    [
        new("MoMo", "momo")
    ];

    public int Order => 2;

    public async Task<int> SeedAsync(AppDbContext context)
    {
        var existingNames = await context.PaymentMethods
            .Select(method => method.Name)
            .ToListAsync();

        var existingKeys = new HashSet<string>(existingNames, StringComparer.OrdinalIgnoreCase);

        var added = 0;
        foreach (var method in PaymentMethods)
        {
            if (!existingKeys.Add(method.Name))
                continue;

            context.PaymentMethods.Add(new PaymentMethodEntity
            {
                Name = method.Name,
                Icon = method.Icon
            });
            added++;
        }

        if (added > 0)
            await context.SaveChangesAsync();

        return added;
    }

    private sealed record PaymentMethodDefinition(string Name, string? Icon);
}
