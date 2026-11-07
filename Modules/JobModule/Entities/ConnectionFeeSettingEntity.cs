using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.JobModule.Entities;

[Table("connection_fee_settings")]
public sealed class ConnectionFeeSettingEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("fee_coin")]
    public int FeeCoin { get; set; }

    [Required]
    [Column("effective_from")]
    public DateTimeOffset EffectiveFrom { get; set; }

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [Column("created_by")]
    public int CreatedBy { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Creator { get; set; } = null!;
}
