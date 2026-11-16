using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class ApplicationStatusJsonConverter : JsonConverter<ApplicationStatus>
{
    public override ApplicationStatus Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Giá trị enum phải là chuỗi.");

        return reader.GetString() switch
        {
            "PENDING" => ApplicationStatus.Pending,
            "ACCEPTED" => ApplicationStatus.Accepted,
            "FINISHED" => ApplicationStatus.Finished,
            "REJECTED" => ApplicationStatus.Rejected,
            "CANCELLED" => ApplicationStatus.Cancelled,
            "NO_SHOW" => ApplicationStatus.NoShow,
            _ => throw new JsonException("Trạng thái ứng tuyển không hợp lệ.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ApplicationStatus value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            ApplicationStatus.Pending => "PENDING",
            ApplicationStatus.Accepted => "ACCEPTED",
            ApplicationStatus.Finished => "FINISHED",
            ApplicationStatus.Rejected => "REJECTED",
            ApplicationStatus.Cancelled => "CANCELLED",
            ApplicationStatus.NoShow => "NO_SHOW",
            _ => throw new JsonException($"Trạng thái ứng tuyển không được hỗ trợ: {value}.")
        });
}
