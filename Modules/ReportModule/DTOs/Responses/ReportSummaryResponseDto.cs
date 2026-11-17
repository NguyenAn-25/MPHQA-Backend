using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

public sealed record ReportSummaryResponseDto(
    int Id,
    ReportObjectType ObjectType,
    int ObjectId,
    string Title,
    string Reason,
    ReportStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ProcessedAt,
    ReportSenderResponseDto Sender);
