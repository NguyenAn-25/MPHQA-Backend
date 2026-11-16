using DotNetSkeleton.Modules.WalletModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.WalletModule.Converters;

public sealed class WalletObjectTypeValueConverter : ValueConverter<WalletObjectType?, string?>
{
    public WalletObjectTypeValueConverter()
        : base(
            objectType => ToStorageValue(objectType),
            value => FromStorageValue(value))
    {
    }

    private static string? ToStorageValue(WalletObjectType? objectType) =>
        objectType switch
        {
            null => null,
            WalletObjectType.TopupOrder => "TOPUP_ORDER",
            WalletObjectType.Job => "JOB",
            WalletObjectType.Application => "APPLICATION",
            _ => throw new InvalidOperationException($"Unsupported wallet object type: {objectType}.")
        };

    private static WalletObjectType? FromStorageValue(string? value) =>
        value switch
        {
            null => null,
            "TOPUP_ORDER" => WalletObjectType.TopupOrder,
            "JOB" => WalletObjectType.Job,
            "APPLICATION" => WalletObjectType.Application,
            _ => throw new InvalidOperationException($"Unsupported wallet object type value: {value}.")
        };
}
