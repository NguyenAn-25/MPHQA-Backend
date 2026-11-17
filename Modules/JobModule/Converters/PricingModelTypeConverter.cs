using System.ComponentModel;
using System.Globalization;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class PricingModelTypeConverter : EnumConverter
{
    public PricingModelTypeConverter() : base(typeof(PricingModel))
    {
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string text)
            return base.ConvertFrom(context, culture, value);

        return text.Trim().ToUpperInvariant() switch
        {
            "TIME_BASED" => PricingModel.TimeBased,
            "PER_CONNECTION" => PricingModel.PerConnection,
            _ => throw new FormatException("Invalid pricing model.")
        };
    }
}
