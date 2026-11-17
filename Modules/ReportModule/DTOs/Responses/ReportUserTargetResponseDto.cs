namespace DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

public sealed record ReportUserTargetResponseDto(
    int Id,
    Guid PublicId,
    string Fullname,
    string Phone,
    bool IsActive);
