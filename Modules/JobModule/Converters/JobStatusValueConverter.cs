using DotNetSkeleton.Modules.JobModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class JobStatusValueConverter : ValueConverter<JobStatus, string>
{
    public JobStatusValueConverter()
        : base(status => ToStorageValue(status), value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(JobStatus status) => status switch
    {
        JobStatus.Pending => "PENDING",
        JobStatus.Accepted => "ACCEPTED",
        JobStatus.Rejected => "REJECTED",
        JobStatus.Full => "FULL",
        JobStatus.Working => "WORKING",
        JobStatus.Completed => "COMPLETED",
        JobStatus.Cancelled => "CANCELLED",
        _ => throw new InvalidOperationException($"Unsupported job status: {status}.")
    };

    private static JobStatus FromStorageValue(string value) => value.Trim().ToUpperInvariant() switch
    {
        "PENDING" => JobStatus.Pending,
        "ACCEPTED" => JobStatus.Accepted,
        "REJECTED" => JobStatus.Rejected,
        "FULL" => JobStatus.Full,
        "WORKING" => JobStatus.Working,
        "COMPLETED" => JobStatus.Completed,
        "CANCELLED" => JobStatus.Cancelled,
        _ => throw new InvalidOperationException($"Unsupported job status value: {value}.")
    };
}
