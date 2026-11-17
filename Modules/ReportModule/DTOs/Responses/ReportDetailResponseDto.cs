using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

public sealed record ReportDetailResponseDto(
    int Id,
    ReportObjectType ObjectType,
    int ObjectId,
    string Title,
    string Reason,
    string? Image,
    ReportStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ProcessedAt,
    ReportSenderResponseDto Sender,
    ReportTargetResponseDto Target);
