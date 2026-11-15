using DotNetSkeleton.Modules.WalletModule.DTOs.Responses;
using DotNetSkeleton.Modules.WalletModule.Services.Interfaces;
using DotNetSkeleton.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.WalletModule.Services;

public sealed class PaymentMethodService(AppDbContext context) : IPaymentMethodService
{
    public async Task<IReadOnlyList<PaymentMethodResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.PaymentMethods
            .AsNoTracking()
            .OrderBy(method => method.Id)
            .Select(method => new PaymentMethodResponseDto(
                method.Id,
                method.Name,
                method.Icon))
            .ToListAsync(cancellationToken);
    }
}
