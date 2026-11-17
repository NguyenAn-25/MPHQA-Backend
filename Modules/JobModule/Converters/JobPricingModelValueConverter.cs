using DotNetSkeleton.Modules.JobModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class JobPricingModelValueConverter : ValueConverter<JobPricingModel, string>
{
    public JobPricingModelValueConverter()
        : base(model => ToStorageValue(model), value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(JobPricingModel model) => model switch
    {
        JobPricingModel.TimeBased => "TIME_BASED",
        JobPricingModel.PerConnection => "PER_CONNECTION",
        _ => throw new InvalidOperationException($"Unsupported job pricing model: {model}.")
    };

    private static JobPricingModel FromStorageValue(string value) => value.Trim().ToUpperInvariant() switch
    {
        "TIME_BASED" => JobPricingModel.TimeBased,
        "PER_CONNECTION" => JobPricingModel.PerConnection,
        _ => throw new InvalidOperationException($"Unsupported job pricing model value: {value}.")
    };
}
