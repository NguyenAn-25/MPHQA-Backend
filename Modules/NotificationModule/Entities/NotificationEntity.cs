using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.NotificationModule.Entities;

[Table("notifications")]
public sealed class NotificationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = string.Empty;

    [Required]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [Column("send_to")]
    public string SendTo { get; set; } = string.Empty;

    [Required]
    [Column("created_by")]
    public int CreatedBy { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Creator { get; set; } = null!;

    public ICollection<NotificationDetailEntity> Details { get; set; } = new List<NotificationDetailEntity>();
}
