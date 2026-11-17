using DotNetSkeleton.Modules.ReportModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.ReportModule.Converters;

public sealed class ReportObjectTypeValueConverter : ValueConverter<ReportObjectType, string>
{
    public ReportObjectTypeValueConverter()
        : base(type => ToStorageValue(type), value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(ReportObjectType type) => type switch
    {
        ReportObjectType.Job => "JOB",
        ReportObjectType.User => "USER",
        _ => throw new InvalidOperationException($"Unsupported report object type: {type}.")
    };

    private static ReportObjectType FromStorageValue(string value) => value switch
    {
        "JOB" => ReportObjectType.Job,
        "USER" => ReportObjectType.User,
        _ => throw new InvalidOperationException($"Unsupported report object type value: {value}.")
    };
}
