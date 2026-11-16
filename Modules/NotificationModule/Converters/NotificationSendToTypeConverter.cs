using System.ComponentModel;
using System.Globalization;
using DotNetSkeleton.Modules.NotificationModule.Enums;

namespace DotNetSkeleton.Modules.NotificationModule.Converters;

public sealed class NotificationSendToTypeConverter : EnumConverter
{
    public NotificationSendToTypeConverter() : base(typeof(NotificationSendTo))
    {
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string text)
        {
            return text.Trim().ToUpperInvariant() switch
            {
                "ALL" or "ALL_USERS" or "TẤT CẢ NGƯỜI DÙNG" => NotificationSendTo.All,
                "VERIFIED_USERS" or "NGƯỜI DÙNG ĐÃ XÁC THỰC" => NotificationSendTo.VerifiedUsers,
                "SPECIFIC_USER" or "USER" or "NGƯỜI DÙNG CỤ THỂ" => NotificationSendTo.SpecificUser,
                _ => throw new FormatException(
                    "sendTo chỉ chấp nhận ALL (Tất cả người dùng), VERIFIED_USERS (Người dùng đã xác thực) hoặc SPECIFIC_USER (Người dùng cụ thể).")
            };
        }

        return base.ConvertFrom(context, culture, value);
    }
}
