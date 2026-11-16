using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class JobStatusJsonConverter : JsonConverter<JobStatus>
{
    public override JobStatus Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Giá trị enum phải là chuỗi.");

        return reader.GetString() switch
        {
            "PENDING" => JobStatus.Pending,
            "ACCEPTED" => JobStatus.Accepted,
            "WORKING" => JobStatus.Working,
            "COMPLETED" => JobStatus.Completed,
            "REJECTED" => JobStatus.Rejected,
            "CANCELLED" => JobStatus.Cancelled,
            _ => throw new JsonException("Trạng thái công việc không hợp lệ.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        JobStatus value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            JobStatus.Pending => "PENDING",
            JobStatus.Accepted => "ACCEPTED",
            JobStatus.Working => "WORKING",
            JobStatus.Completed => "COMPLETED",
            JobStatus.Rejected => "REJECTED",
            JobStatus.Cancelled => "CANCELLED",
            _ => throw new JsonException($"Trạng thái công việc không được hỗ trợ: {value}.")
        });
}
