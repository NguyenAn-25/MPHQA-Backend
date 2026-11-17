using System.ComponentModel;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.ReportModule.Converters;

namespace DotNetSkeleton.Modules.ReportModule.Enums;

[JsonConverter(typeof(ReportObjectTypeJsonConverter))]
[TypeConverter(typeof(ReportObjectTypeTypeConverter))]
public enum ReportObjectType
{
    Job,
    User
}
