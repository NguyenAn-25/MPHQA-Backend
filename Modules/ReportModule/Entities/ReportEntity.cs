using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string ObjectType { get; set; } = string.Empty;

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
    public string Status { get; set; } = "PENDING";

    [Column("image")]
    public string? Image { get; set; }

    [Required]
    [Column("sender_id")]
    public int SenderId { get; set; }

    [ForeignKey(nameof(SenderId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Sender { get; set; } = null!;
}
