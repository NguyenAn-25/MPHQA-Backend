using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class PricingModelJsonConverter : JsonConverter<PricingModel>
{
    public override PricingModel Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Giá trị enum phải là chuỗi.");

        return reader.GetString() switch
        {
            "TIME_BASED" => PricingModel.TimeBased,
            "PER_CONNECTION" => PricingModel.PerConnection,
            _ => throw new JsonException("Mô hình định giá không hợp lệ.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        PricingModel value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            PricingModel.TimeBased => "TIME_BASED",
            PricingModel.PerConnection => "PER_CONNECTION",
            _ => throw new JsonException($"Mô hình định giá không được hỗ trợ: {value}.")
        });
}
