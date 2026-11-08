using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.ProfileModule.Entities;

[Table("skills")]
public sealed class SkillEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("icon")]
    public string? Icon { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    public ICollection<ProfileSkillEntity> ProfileSkills { get; set; } = new List<ProfileSkillEntity>();
    public ICollection<DotNetSkeleton.Modules.JobModule.Entities.JobRequiredSkillEntity> JobRequiredSkills { get; set; } = new List<DotNetSkeleton.Modules.JobModule.Entities.JobRequiredSkillEntity>();
}
