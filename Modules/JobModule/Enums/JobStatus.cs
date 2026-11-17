using System.ComponentModel;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.JobModule.Converters;

namespace DotNetSkeleton.Modules.JobModule.Enums;

[JsonConverter(typeof(JobStatusJsonConverter))]
[TypeConverter(typeof(JobStatusTypeConverter))]
public enum JobStatus
{
    Pending,
    Accepted,
    Working,
    Completed,
    Rejected,
    Cancelled
}
