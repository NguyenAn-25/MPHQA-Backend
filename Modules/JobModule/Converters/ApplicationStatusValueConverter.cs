using DotNetSkeleton.Modules.JobModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class ApplicationStatusValueConverter : ValueConverter<ApplicationStatus, string>
{
    public ApplicationStatusValueConverter()
        : base(
            status => ToStorageValue(status),
            value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(ApplicationStatus status) =>
        status switch
        {
            ApplicationStatus.Pending => "PENDING",
            ApplicationStatus.Accepted => "ACCEPTED",
            ApplicationStatus.Finished => "FINISHED",
            ApplicationStatus.Rejected => "REJECTED",
            ApplicationStatus.Cancelled => "CANCELLED",
            ApplicationStatus.NoShow => "NO_SHOW",
            _ => throw new InvalidOperationException($"Unsupported application status: {status}.")
        };

    private static ApplicationStatus FromStorageValue(string value) =>
        value switch
        {
            "PENDING" => ApplicationStatus.Pending,
            "ACCEPTED" => ApplicationStatus.Accepted,
            "FINISHED" => ApplicationStatus.Finished,
            "REJECTED" => ApplicationStatus.Rejected,
            "CANCELLED" => ApplicationStatus.Cancelled,
            "NO_SHOW" => ApplicationStatus.NoShow,
            _ => throw new InvalidOperationException($"Unsupported application status value: {value}.")
        };
}
