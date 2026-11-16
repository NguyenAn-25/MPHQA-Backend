using DotNetSkeleton.Modules.WalletModule.DTOs.Responses;

namespace DotNetSkeleton.Modules.WalletModule.Services.Interfaces;

public interface IPaymentMethodService
{
    Task<IReadOnlyList<PaymentMethodResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
