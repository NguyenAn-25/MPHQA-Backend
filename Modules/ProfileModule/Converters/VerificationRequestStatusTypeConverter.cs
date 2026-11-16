using System.ComponentModel;
using System.Globalization;
using DotNetSkeleton.Modules.ProfileModule.Enums;

namespace DotNetSkeleton.Modules.ProfileModule.Converters;

public sealed class VerificationRequestStatusTypeConverter : EnumConverter
{
    public VerificationRequestStatusTypeConverter() : base(typeof(VerificationRequestStatus))
    {
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string text)
        {
            var candidate = text.Trim();
            if (Matches(candidate, "Đang chờ duyệt", "PENDING", "Pending"))
                return VerificationRequestStatus.Pending;
            if (Matches(candidate, "Đã chấp nhận", "ACCEPTED", "Accepted"))
                return VerificationRequestStatus.Accepted;
            if (Matches(candidate, "Đã từ chối", "REJECTED", "Rejected"))
                return VerificationRequestStatus.Rejected;
            if (Matches(candidate, "Đã hủy", "CANCELLED", "Cancelled"))
                return VerificationRequestStatus.Cancelled;

            throw new FormatException(
                "Status phải là Đang chờ duyệt, Đã chấp nhận, Đã từ chối hoặc Đã hủy.");
        }

        return base.ConvertFrom(context, culture, value);
    }

    private static bool Matches(string value, params string[] acceptedValues) =>
        acceptedValues.Any(accepted => string.Equals(value, accepted, StringComparison.OrdinalIgnoreCase));
}
