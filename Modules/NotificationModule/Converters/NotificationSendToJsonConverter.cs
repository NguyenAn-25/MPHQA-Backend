using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.NotificationModule.Enums;

namespace DotNetSkeleton.Modules.NotificationModule.Converters;

public sealed class NotificationSendToJsonConverter : JsonConverter<NotificationSendTo>
{
    public override NotificationSendTo Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("sendTo phải là chuỗi.");

        return reader.GetString()?.Trim().ToUpperInvariant() switch
        {
            "ALL" or "ALL_USERS" or "TẤT CẢ NGƯỜI DÙNG" => NotificationSendTo.All,
            "VERIFIED_USERS" or "NGƯỜI DÙNG ĐÃ XÁC THỰC" => NotificationSendTo.VerifiedUsers,
            "SPECIFIC_USER" or "USER" or "NGƯỜI DÙNG CỤ THỂ" => NotificationSendTo.SpecificUser,
            _ => throw new JsonException(
                "sendTo chỉ chấp nhận ALL (Tất cả người dùng), VERIFIED_USERS (Người dùng đã xác thực) hoặc SPECIFIC_USER (Người dùng cụ thể).")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        NotificationSendTo value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            NotificationSendTo.All => "Tất cả người dùng",
            NotificationSendTo.VerifiedUsers => "Người dùng đã xác thực",
            NotificationSendTo.SpecificUser => "Người dùng cụ thể",
            _ => throw new JsonException($"sendTo không được hỗ trợ: {value}.")
        });
}
