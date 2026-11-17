using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.Converters;

public sealed class ReportObjectTypeJsonConverter : JsonConverter<ReportObjectType>
{
    public override ReportObjectType Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("objectType phải là chuỗi.");

        return reader.GetString()?.Trim().ToUpperInvariant() switch
        {
            "JOB" or "CÔNG VIỆC" => ReportObjectType.Job,
            "USER" or "NGƯỜI DÙNG" => ReportObjectType.User,
            _ => throw new JsonException("objectType chỉ chấp nhận JOB (Công việc) hoặc USER (Người dùng).")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ReportObjectType value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            ReportObjectType.Job => "Công việc",
            ReportObjectType.User => "Người dùng",
            _ => throw new JsonException($"objectType không được hỗ trợ: {value}.")
        });
}
