namespace DotNetSkeleton.Modules.JobModule.DTOs.Responses;

public sealed record JobAuthorResponseDto(
    int Id,
    Guid PublicId,
    string Phone,
    string Fullname,
    string? Avatar);
