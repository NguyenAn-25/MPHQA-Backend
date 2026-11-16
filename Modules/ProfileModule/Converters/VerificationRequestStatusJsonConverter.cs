using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetSkeleton.Modules.ProfileModule.Enums;

namespace DotNetSkeleton.Modules.ProfileModule.Converters;

public sealed class VerificationRequestStatusJsonConverter : JsonConverter<VerificationRequestStatus>
{
    public override VerificationRequestStatus Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Trạng thái yêu cầu xác thực phải là chuỗi.");

        return reader.GetString() switch
        {
            "PENDING" or "Đang chờ duyệt" => VerificationRequestStatus.Pending,
            "ACCEPTED" or "Đã chấp nhận" => VerificationRequestStatus.Accepted,
            "REJECTED" or "Đã từ chối" => VerificationRequestStatus.Rejected,
            "CANCELLED" or "Đã hủy" => VerificationRequestStatus.Cancelled,
            _ => throw new JsonException("Trạng thái yêu cầu xác thực không hợp lệ.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        VerificationRequestStatus value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            VerificationRequestStatus.Pending => "Đang chờ duyệt",
            VerificationRequestStatus.Accepted => "Đã chấp nhận",
            VerificationRequestStatus.Rejected => "Đã từ chối",
            VerificationRequestStatus.Cancelled => "Đã hủy",
            _ => throw new JsonException($"Trạng thái yêu cầu xác thực không được hỗ trợ: {value}.")
        });
}
