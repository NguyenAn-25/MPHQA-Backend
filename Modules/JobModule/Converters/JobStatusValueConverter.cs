using DotNetSkeleton.Modules.JobModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class JobStatusValueConverter : ValueConverter<JobStatus, string>
{
    public JobStatusValueConverter()
        : base(
            status => ToStorageValue(status),
            value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(JobStatus status) =>
        status switch
        {
            JobStatus.Pending => "PENDING",
            JobStatus.Accepted => "ACCEPTED",
            JobStatus.Working => "WORKING",
            JobStatus.Completed => "COMPLETED",
            JobStatus.Rejected => "REJECTED",
            JobStatus.Cancelled => "CANCELLED",
            _ => throw new InvalidOperationException($"Unsupported job status: {status}.")
        };

    private static JobStatus FromStorageValue(string value) =>
        value switch
        {
            "PENDING" => JobStatus.Pending,
            "ACCEPTED" => JobStatus.Accepted,
            "WORKING" => JobStatus.Working,
            "COMPLETED" => JobStatus.Completed,
            "REJECTED" => JobStatus.Rejected,
            "CANCELLED" => JobStatus.Cancelled,
            _ => throw new InvalidOperationException($"Unsupported job status value: {value}.")
        };
}
