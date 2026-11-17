using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class JobPricingModelJsonConverter : JsonConverter<JobPricingModel>
{
    public override JobPricingModel Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("pricingModel phải là chuỗi.");

        return reader.GetString()?.Trim().ToUpperInvariant() switch
        {
            "TIME_BASED" or "THEO THỜI GIAN" => JobPricingModel.TimeBased,
            "PER_CONNECTION" or "THEO LƯỢT KẾT NỐI" => JobPricingModel.PerConnection,
            _ => throw new JsonException("pricingModel không hợp lệ.")
        };
    }

    public override void Write(Utf8JsonWriter writer, JobPricingModel value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            JobPricingModel.TimeBased => "Theo thời gian",
            JobPricingModel.PerConnection => "Theo lượt kết nối",
            _ => throw new JsonException($"Mô hình tính phí không được hỗ trợ: {value}.")
        });
}
