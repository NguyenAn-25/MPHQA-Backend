using DotNetSkeleton.Modules.JobModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class PricingModelValueConverter : ValueConverter<PricingModel, string>
{
    public PricingModelValueConverter()
        : base(
            pricingModel => ToStorageValue(pricingModel),
            value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(PricingModel pricingModel) =>
        pricingModel switch
        {
            PricingModel.TimeBased => "TIME_BASED",
            PricingModel.PerConnection => "PER_CONNECTION",
            _ => throw new InvalidOperationException($"Unsupported pricing model: {pricingModel}.")
        };

    private static PricingModel FromStorageValue(string value) =>
        value switch
        {
            "TIME_BASED" => PricingModel.TimeBased,
            "PER_CONNECTION" => PricingModel.PerConnection,
            _ => throw new InvalidOperationException($"Unsupported pricing model value: {value}.")
        };
}
