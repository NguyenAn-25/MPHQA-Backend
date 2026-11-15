namespace DotNetSkeleton.Modules.WalletModule.DTOs.Responses;

/// <summary>
/// Represents a payment method available for wallet transactions.
/// </summary>
public sealed record PaymentMethodResponseDto(
    int Id,
    string Name,
    string? Icon);
