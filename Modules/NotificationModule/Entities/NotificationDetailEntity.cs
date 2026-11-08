using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.NotificationModule.Entities;

[Table("notification_details")]
[PrimaryKey(nameof(NotificationId), nameof(UserId))]
public sealed class NotificationDetailEntity
{
    [Column("read_at")]
    public DateTimeOffset? ReadAt { get; set; }

    [Column("notification_id")]
    public int NotificationId { get; set; }

    [ForeignKey(nameof(NotificationId))]
    public NotificationEntity Notification { get; set; } = null!;

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity User { get; set; } = null!;
}
