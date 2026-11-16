using System.ComponentModel;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.ProfileModule.Converters;

namespace DotNetSkeleton.Modules.ProfileModule.Enums;

[JsonConverter(typeof(VerificationRequestStatusJsonConverter))]
[TypeConverter(typeof(VerificationRequestStatusTypeConverter))]
public enum VerificationRequestStatus
{
    Pending,
    Accepted,
    Rejected,
    Cancelled
}
