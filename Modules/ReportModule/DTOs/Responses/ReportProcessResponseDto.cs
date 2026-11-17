using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

public sealed record ReportProcessResponseDto(
    int Id,
    ReportStatus Status,
    DateTimeOffset ProcessedAt,
    string? Note);
