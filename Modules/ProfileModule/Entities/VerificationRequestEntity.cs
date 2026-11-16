using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetSkeleton.Modules.ProfileModule.Enums;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.ProfileModule.Entities;

[Table("verification_requests")]
[Index(nameof(Status), nameof(CreatedAt))]
public sealed class VerificationRequestEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("full_name_on_card")]
    public string FullNameOnCard { get; set; } = string.Empty;

    [Required]
    [Column("id_number")]
    public string IdNumber { get; set; } = string.Empty;

    [Required]
    [Column("card_front_image")]
    public string CardFrontImage { get; set; } = string.Empty;

    [Required]
    [Column("card_back_image")]
    public string CardBackImage { get; set; } = string.Empty;

    [Required]
    [Column("portrait_image")]
    public string PortraitImage { get; set; } = string.Empty;

    [Required]
    [Column("status")]
    public VerificationRequestStatus Status { get; set; } = VerificationRequestStatus.Pending;

    [Column("rejection_reason", TypeName = "text")]
    public string? RejectionReason { get; set; }

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("verified_at")]
    public DateTimeOffset? VerifiedAt { get; set; }

    [Column("rejected_at")]
    public DateTimeOffset? RejectedAt { get; set; }

    [Required]
    [Column("sender_id")]
    public int SenderId { get; set; }

    [ForeignKey(nameof(SenderId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Sender { get; set; } = null!;
}
