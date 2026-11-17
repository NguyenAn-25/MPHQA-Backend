using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

public sealed record ReportJobTargetResponseDto(
    int Id,
    string Title,
    JobStatus Status,
    DateTimeOffset CreatedAt);
