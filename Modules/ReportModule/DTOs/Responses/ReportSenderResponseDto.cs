namespace DotNetSkeleton.Modules.ReportModule.DTOs.Responses;

public sealed record ReportSenderResponseDto(
    int Id,
    Guid PublicId,
    string Phone,
    string Fullname,
    string? Avatar);
