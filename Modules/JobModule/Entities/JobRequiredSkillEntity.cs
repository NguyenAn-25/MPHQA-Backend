using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.JobModule.Entities;

[Table("job_required_skills")]
public sealed class JobRequiredSkillEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("job_id")]
    public int JobId { get; set; }

    [ForeignKey(nameof(JobId))]
    public JobEntity Job { get; set; } = null!;

    [Required]
    [Column("skill_id")]
    public int SkillId { get; set; }

    [ForeignKey(nameof(SkillId))]
    public DotNetSkeleton.Modules.ProfileModule.Entities.SkillEntity Skill { get; set; } = null!;
}
