using System.ComponentModel;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.ReportModule.Converters;

namespace DotNetSkeleton.Modules.ReportModule.Enums;

[JsonConverter(typeof(ReportStatusJsonConverter))]
[TypeConverter(typeof(ReportStatusTypeConverter))]
public enum ReportStatus
{
    Pending,
    Resolved,
    Rejected,
    Cancelled
}
