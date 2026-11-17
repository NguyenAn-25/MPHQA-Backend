using DotNetSkeleton.Modules.ReportModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.ReportModule.Converters;

public sealed class ReportStatusValueConverter : ValueConverter<ReportStatus, string>
{
    public ReportStatusValueConverter()
        : base(status => ToStorageValue(status), value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(ReportStatus status) => status switch
    {
        ReportStatus.Pending => "PENDING",
        ReportStatus.Resolved => "RESOLVED",
        ReportStatus.Rejected => "REJECTED",
        ReportStatus.Cancelled => "CANCELLED",
        _ => throw new InvalidOperationException($"Unsupported report status: {status}.")
    };

    private static ReportStatus FromStorageValue(string value) => value switch
    {
        "PENDING" => ReportStatus.Pending,
        "RESOLVED" => ReportStatus.Resolved,
        "REJECTED" => ReportStatus.Rejected,
        "CANCELLED" => ReportStatus.Cancelled,
        _ => throw new InvalidOperationException($"Unsupported report status value: {value}.")
    };
}
