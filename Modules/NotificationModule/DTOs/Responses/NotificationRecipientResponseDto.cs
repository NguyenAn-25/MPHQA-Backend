namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Responses;

public sealed record NotificationRecipientResponseDto(
    int Id,
    DateTimeOffset? ReadAt,
    NotificationRecipientUserResponseDto User);
