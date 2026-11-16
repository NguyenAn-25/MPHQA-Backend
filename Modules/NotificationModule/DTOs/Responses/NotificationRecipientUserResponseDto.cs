namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;

public sealed record NotificationRecipientUserResponseDto(
    int Id,
    Guid PublicId,
    string Phone,
    string Fullname,
    string? Avatar);
