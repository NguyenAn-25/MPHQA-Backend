using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.NotificationModule.Entities;

[Table("notification_details")]
public sealed class NotificationDetailEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

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
