using System.ComponentModel;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.JobModule.Converters;

namespace DotNetSkeleton.Modules.JobModule.Enums;

[JsonConverter(typeof(PricingModelJsonConverter))]
[TypeConverter(typeof(PricingModelTypeConverter))]
public enum PricingModel
{
    TimeBased,
    PerConnection
}
