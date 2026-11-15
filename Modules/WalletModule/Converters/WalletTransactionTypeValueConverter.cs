using DotNetSkeleton.Modules.WalletModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.WalletModule.Converters;

public sealed class WalletTransactionTypeValueConverter : ValueConverter<WalletTransactionType, string>
{
    public WalletTransactionTypeValueConverter()
        : base(
            transactionType => ToStorageValue(transactionType),
            value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(WalletTransactionType transactionType) =>
        transactionType switch
        {
            WalletTransactionType.Topup => "TOPUP",
            WalletTransactionType.PostFee => "POST_FEE",
            WalletTransactionType.ConnectionFee => "CONNECTION_FEE",
            WalletTransactionType.Refund => "REFUND",
            _ => throw new InvalidOperationException($"Unsupported wallet transaction type: {transactionType}.")
        };

    private static WalletTransactionType FromStorageValue(string value) =>
        value switch
        {
            "TOPUP" => WalletTransactionType.Topup,
            "POST_FEE" => WalletTransactionType.PostFee,
            "CONNECTION_FEE" => WalletTransactionType.ConnectionFee,
            "REFUND" => WalletTransactionType.Refund,
            _ => throw new InvalidOperationException($"Unsupported wallet transaction type value: {value}.")
        };
}
