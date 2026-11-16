using DotNetSkeleton.Modules.NotificationModule.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotNetSkeleton.Modules.NotificationModule.Converters;

public sealed class NotificationSendToValueConverter : ValueConverter<NotificationSendTo, string>
{
    public NotificationSendToValueConverter()
        : base(
            sendTo => ToStorageValue(sendTo),
            value => FromStorageValue(value))
    {
    }

    private static string ToStorageValue(NotificationSendTo sendTo) =>
        sendTo switch
        {
            NotificationSendTo.All => "ALL_USERS",
            NotificationSendTo.VerifiedUsers => "VERIFIED_USERS",
            NotificationSendTo.SpecificUser => "USER",
            _ => throw new InvalidOperationException($"Unsupported notification sendTo value: {sendTo}.")
        };

    private static NotificationSendTo FromStorageValue(string value) =>
        value switch
        {
            "ALL" or "ALL_USERS" or "TẤT CẢ NGƯỜI DÙNG" => NotificationSendTo.All,
            "VERIFIED_USERS" or "NGƯỜI DÙNG ĐÃ XÁC THỰC" => NotificationSendTo.VerifiedUsers,
            "SPECIFIC_USER" or "USER" or "NGƯỜI DÙNG CỤ THỂ" => NotificationSendTo.SpecificUser,
            _ => throw new InvalidOperationException($"Unsupported notification sendTo value: {value}.")
        };
}
