using System.ComponentModel;
using System.Globalization;
using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.Converters;

public sealed class ReportObjectTypeTypeConverter : EnumConverter
{
    public ReportObjectTypeTypeConverter() : base(typeof(ReportObjectType))
    {
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string text)
        {
            return text.Trim().ToUpperInvariant() switch
            {
                "JOB" or "CÔNG VIỆC" => ReportObjectType.Job,
                "USER" or "NGƯỜI DÙNG" => ReportObjectType.User,
                _ => throw new FormatException("objectType phải là JOB (Công việc) hoặc USER (Người dùng).")
            };
        }

        return base.ConvertFrom(context, culture, value);
    }
}
