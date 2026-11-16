using DotNetSkeleton.Modules.WalletModule.DTOs.Responses;
using DotNetSkeleton.Modules.WalletModule.Services.Interfaces;
using DotNetSkeleton.Shared.Api;
using DotNetSkeleton.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSkeleton.Modules.WalletModule.Controllers;

[ApiController]
[Route("api/v1/payment-methods")]
public sealed class PaymentMethodController(IPaymentMethodService paymentMethodService) : ApiControllerBase
{
    /// <summary>
    /// Returns all available payment methods.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<IReadOnlyList<PaymentMethodResponseDto>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PaymentMethodResponseDto>>>> GetAll(
        CancellationToken cancellationToken) =>
        OkResponse(await paymentMethodService.GetAllAsync(cancellationToken));
}
