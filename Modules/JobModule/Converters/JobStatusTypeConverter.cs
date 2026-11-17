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
            "PENDING" => JobStatus.Pending,
            "ACCEPTED" => JobStatus.Accepted,
            "WORKING" => JobStatus.Working,
            "COMPLETED" => JobStatus.Completed,
            "REJECTED" => JobStatus.Rejected,
            "CANCELLED" => JobStatus.Cancelled,
            _ => throw new FormatException("Invalid job status.")
        };
    }
}
