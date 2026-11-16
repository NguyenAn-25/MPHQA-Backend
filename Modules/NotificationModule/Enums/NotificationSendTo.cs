using System.ComponentModel;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.NotificationModule.Converters;

namespace DotNetSkeleton.Modules.NotificationModule.Enums;

[JsonConverter(typeof(NotificationSendToJsonConverter))]
[TypeConverter(typeof(NotificationSendToTypeConverter))]
public enum NotificationSendTo
{
    All,
    VerifiedUsers,
    SpecificUser
}
