using System.ComponentModel;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.JobModule.Converters;

namespace DotNetSkeleton.Modules.JobModule.Enums;

[JsonConverter(typeof(JobPricingModelJsonConverter))]
[TypeConverter(typeof(JobPricingModelTypeConverter))]
public enum JobPricingModel
{
    TimeBased,
    PerConnection
}
