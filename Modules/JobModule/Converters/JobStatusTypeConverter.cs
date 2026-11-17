using System.ComponentModel;
using System.Globalization;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class JobStatusTypeConverter : EnumConverter
{
    public JobStatusTypeConverter() : base(typeof(JobStatus))
    {
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string text)
            return base.ConvertFrom(context, culture, value);

        return text.Trim().ToUpperInvariant() switch
        {
            "PENDING" or "ĐANG CHỜ DUYỆT" => JobStatus.Pending,
            "ACCEPTED" or "ĐÃ DUYỆT" => JobStatus.Accepted,
            "REJECTED" or "ĐÃ TỪ CHỐI" => JobStatus.Rejected,
            "FULL" or "ĐÃ ĐỦ NGƯỜI" => JobStatus.Full,
            "WORKING" or "ĐANG LÀM" => JobStatus.Working,
            "COMPLETED" or "ĐÃ HOÀN THÀNH" => JobStatus.Completed,
            "CANCELLED" or "ĐÃ HỦY" => JobStatus.Cancelled,
            _ => throw new FormatException("status công việc không hợp lệ.")
        };
    }
}
