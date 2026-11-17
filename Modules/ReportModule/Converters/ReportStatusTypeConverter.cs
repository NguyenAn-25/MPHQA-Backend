using System.ComponentModel;
using System.Globalization;
using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.Converters;

public sealed class ReportStatusTypeConverter : EnumConverter
{
    public ReportStatusTypeConverter() : base(typeof(ReportStatus))
    {
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string text)
        {
            return text.Trim().ToUpperInvariant() switch
            {
                "PENDING" or "ĐANG CHỜ XỬ LÝ" => ReportStatus.Pending,
                "RESOLVED" or "ĐÃ XỬ LÝ" => ReportStatus.Resolved,
                "REJECTED" or "ĐÃ TỪ CHỐI" => ReportStatus.Rejected,
                "CANCELLED" or "ĐÃ HỦY" => ReportStatus.Cancelled,
                _ => throw new FormatException(
                    "status phải là PENDING, RESOLVED, REJECTED hoặc CANCELLED và nhãn tiếng Việt tương ứng.")
            };
        }

        return base.ConvertFrom(context, culture, value);
    }
}
