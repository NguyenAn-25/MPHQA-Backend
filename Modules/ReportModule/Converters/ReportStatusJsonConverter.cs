using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.Converters;

public sealed class ReportStatusJsonConverter : JsonConverter<ReportStatus>
{
    public override ReportStatus Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("status phải là chuỗi.");

        return reader.GetString()?.Trim().ToUpperInvariant() switch
        {
            "PENDING" or "ĐANG CHỜ XỬ LÝ" => ReportStatus.Pending,
            "RESOLVED" or "ĐÃ XỬ LÝ" => ReportStatus.Resolved,
            "REJECTED" or "ĐÃ TỪ CHỐI" => ReportStatus.Rejected,
            "CANCELLED" or "ĐÃ HỦY" => ReportStatus.Cancelled,
            _ => throw new JsonException(
                "status chỉ chấp nhận PENDING, RESOLVED, REJECTED hoặc CANCELLED và nhãn tiếng Việt tương ứng.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ReportStatus value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            ReportStatus.Pending => "Đang chờ xử lý",
            ReportStatus.Resolved => "Đã xử lý",
            ReportStatus.Rejected => "Đã từ chối",
            ReportStatus.Cancelled => "Đã hủy",
            _ => throw new JsonException($"status báo cáo không được hỗ trợ: {value}.")
        });
}
