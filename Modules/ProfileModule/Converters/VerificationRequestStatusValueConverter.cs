using DotNetSkeleton.Modules.ProfileModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.ProfileModule.Converters;

public sealed class VerificationRequestStatusValueConverter
    : ValueConverter<VerificationRequestStatus, string>
{
    public VerificationRequestStatusValueConverter()
        : base(
            status => ToStorageValue(status),
            value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(VerificationRequestStatus status) =>
        status switch
        {
            VerificationRequestStatus.Pending => "PENDING",
            VerificationRequestStatus.Accepted => "ACCEPTED",
            VerificationRequestStatus.Rejected => "REJECTED",
            VerificationRequestStatus.Cancelled => "CANCELLED",
            _ => throw new InvalidOperationException($"Unsupported verification request status: {status}.")
        };

    private static VerificationRequestStatus FromStorageValue(string value) =>
        value switch
        {
            "PENDING" => VerificationRequestStatus.Pending,
            "ACCEPTED" => VerificationRequestStatus.Accepted,
            "REJECTED" => VerificationRequestStatus.Rejected,
            "CANCELLED" => VerificationRequestStatus.Cancelled,
            _ => throw new InvalidOperationException($"Unsupported verification request status value: {value}.")
        };
}
