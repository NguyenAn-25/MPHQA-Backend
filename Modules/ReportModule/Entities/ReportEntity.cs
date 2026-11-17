using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetSkeleton.Modules.ReportModule.Enums;

namespace DotNetSkeleton.Modules.ReportModule.Entities;

[Table("reports")]
public sealed class ReportEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("object_type")]
    public ReportObjectType ObjectType { get; set; }

    [Required]
    [Column("object_id")]
    public int ObjectId { get; set; }

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("processed_at")]
    public DateTimeOffset? ProcessedAt { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column("reason", TypeName = "text")]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [Column("status")]
    public ReportStatus Status { get; set; } = ReportStatus.Pending;

    [Column("processed_note", TypeName = "text")]
    public string? ProcessedNote { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Required]
    [Column("sender_id")]
    public int SenderId { get; set; }

    [ForeignKey(nameof(SenderId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Sender { get; set; } = null!;
}
