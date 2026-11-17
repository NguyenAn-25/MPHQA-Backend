using System.ComponentModel;
using System.Globalization;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class JobPricingModelTypeConverter : EnumConverter
{
    public JobPricingModelTypeConverter() : base(typeof(JobPricingModel))
    {
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string text)
            return base.ConvertFrom(context, culture, value);

        return text.Trim().ToUpperInvariant() switch
        {
            "TIME_BASED" or "THEO THỜI GIAN" => JobPricingModel.TimeBased,
            "PER_CONNECTION" or "THEO LƯỢT KẾT NỐI" => JobPricingModel.PerConnection,
            _ => throw new FormatException("pricingModel không hợp lệ.")
        };
    }
}
