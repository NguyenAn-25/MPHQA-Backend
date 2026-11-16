using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetSkeleton.Modules.SkillModule.Entities;

namespace DotNetSkeleton.Modules.ProfileModule.Entities;

[Table("profile_skills")]
public sealed class ProfileSkillEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("profile_id")]
    public int ProfileId { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public ProfileEntity Profile { get; set; } = null!;

    [Required]
    [Column("skill_id")]
    public int SkillId { get; set; }

    [ForeignKey(nameof(SkillId))]
    public SkillEntity Skill { get; set; } = null!;
}
