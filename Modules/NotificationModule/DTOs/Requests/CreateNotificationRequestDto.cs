using System.ComponentModel.DataAnnotations;
using DotNetSkeleton.Modules.NotificationModule.Enums;

namespace DotNetSkeleton.Modules.NotificationModule.DTOs.Requests;

public sealed class CreateNotificationRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Content { get; init; } = string.Empty;

    [Required(ErrorMessage = "Đối tượng nhận thông báo là bắt buộc.")]
    public NotificationSendTo? SendTo { get; init; }

    [Range(1, int.MaxValue)]
    public int? UserId { get; init; }
}
