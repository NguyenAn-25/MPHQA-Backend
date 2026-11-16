using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.WalletModule.Enums;

namespace DotNetSkeleton.Modules.WalletModule.Converters;

public sealed class WalletObjectTypeJsonConverter : JsonConverter<WalletObjectType>
{
    public override WalletObjectType Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Giá trị enum phải là chuỗi.");

        return reader.GetString() switch
        {
            "TOPUP_ORDER" => WalletObjectType.TopupOrder,
            "JOB" => WalletObjectType.Job,
            "APPLICATION" => WalletObjectType.Application,
            _ => throw new JsonException("Loại đối tượng ví không hợp lệ.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        WalletObjectType value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            WalletObjectType.TopupOrder => "TOPUP_ORDER",
            WalletObjectType.Job => "JOB",
            WalletObjectType.Application => "APPLICATION",
            _ => throw new JsonException($"Loại đối tượng ví không được hỗ trợ: {value}.")
        });
}
