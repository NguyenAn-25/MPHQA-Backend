using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Converters;

public sealed class JobStatusJsonConverter : JsonConverter<JobStatus>
{
    public override JobStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("status phải là chuỗi.");

        return reader.GetString()?.Trim().ToUpperInvariant() switch
        {
            "PENDING" or "ĐANG CHỜ DUYỆT" => JobStatus.Pending,
            "ACCEPTED" or "ĐÃ DUYỆT" => JobStatus.Accepted,
            "REJECTED" or "ĐÃ TỪ CHỐI" => JobStatus.Rejected,
            "FULL" or "ĐÃ ĐỦ NGƯỜI" => JobStatus.Full,
            "WORKING" or "ĐANG LÀM" => JobStatus.Working,
            "COMPLETED" or "ĐÃ HOÀN THÀNH" => JobStatus.Completed,
            "CANCELLED" or "ĐÃ HỦY" => JobStatus.Cancelled,
            _ => throw new JsonException("status công việc không hợp lệ.")
        };
    }

    public override void Write(Utf8JsonWriter writer, JobStatus value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            JobStatus.Pending => "Đang chờ duyệt",
            JobStatus.Accepted => "Đã duyệt",
            JobStatus.Rejected => "Đã từ chối",
            JobStatus.Full => "Đã đủ người",
            JobStatus.Working => "Đang làm",
            JobStatus.Completed => "Đã hoàn thành",
            JobStatus.Cancelled => "Đã hủy",
            _ => throw new JsonException($"Trạng thái công việc không được hỗ trợ: {value}.")
        });
}
