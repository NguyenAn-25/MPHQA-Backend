using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.JobModule.Entities;

[Table("job_feedbacks")]
public sealed class JobFeedbackEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Required]
    [Column("feedback_at")]
    public DateTimeOffset FeedbackAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [Column("star")]
    public int Star { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Required]
    [Column("reviewer_id")]
    public int ReviewerId { get; set; }

    [ForeignKey(nameof(ReviewerId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Reviewer { get; set; } = null!;

    [Required]
    [Column("reviewee_id")]
    public int RevieweeId { get; set; }

    [ForeignKey(nameof(RevieweeId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Reviewee { get; set; } = null!;

    [Required]
    [Column("job_id")]
    public int JobId { get; set; }

    [ForeignKey(nameof(JobId))]
    public JobEntity Job { get; set; } = null!;
}
