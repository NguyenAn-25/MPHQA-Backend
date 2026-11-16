using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetSkeleton.Modules.JobModule.Enums;

namespace DotNetSkeleton.Modules.JobModule.Entities;

[Table("applications")]
public sealed class ApplicationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("status")]
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    [Required]
    [Column("applied_at")]
    public DateTimeOffset AppliedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("note", TypeName = "text")]
    public string? Note { get; set; }

    [Column("connection_fee_coin")]
    public int? ConnectionFeeCoin { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity User { get; set; } = null!;

    [Required]
    [Column("job_id")]
    public int JobId { get; set; }

    [ForeignKey(nameof(JobId))]
    public JobEntity Job { get; set; } = null!;
}
