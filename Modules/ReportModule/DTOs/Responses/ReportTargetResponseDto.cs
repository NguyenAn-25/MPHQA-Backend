using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

public sealed record ReportTargetResponseDto(
    ReportObjectType Type,
    ReportJobTargetResponseDto? JobDetails,
    ReportUserTargetResponseDto? UserDetails);
