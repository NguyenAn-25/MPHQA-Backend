using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.WalletModule.Enums;

namespace DotNetSkeleton.Modules.WalletModule.Converters;

public sealed class WalletTransactionTypeJsonConverter : JsonConverter<WalletTransactionType>
{
    public override WalletTransactionType Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Giá trị enum phải là chuỗi.");

        return reader.GetString() switch
        {
            "TOPUP" => WalletTransactionType.Topup,
            "POST_FEE" => WalletTransactionType.PostFee,
            "CONNECTION_FEE" => WalletTransactionType.ConnectionFee,
            "REFUND" => WalletTransactionType.Refund,
            _ => throw new JsonException("Loại giao dịch ví không hợp lệ.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        WalletTransactionType value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            WalletTransactionType.Topup => "TOPUP",
            WalletTransactionType.PostFee => "POST_FEE",
            WalletTransactionType.ConnectionFee => "CONNECTION_FEE",
            WalletTransactionType.Refund => "REFUND",
            _ => throw new JsonException($"Loại giao dịch ví không được hỗ trợ: {value}.")
        });
}
