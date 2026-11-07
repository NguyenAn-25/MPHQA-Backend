using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.ProfileModule.Entities;

[Table("profile_experiences")]
public sealed class ProfileExperienceEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("job_title")]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    [Column("company_name")]
    public string CompanyName { get; set; } = string.Empty;

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("title")]
    public string? Title { get; set; }

    [Required]
    [Column("profile_id")]
    public int ProfileId { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public ProfileEntity Profile { get; set; } = null!;

    [Column("job_category_id")]
    public int? JobCategoryId { get; set; }

    [ForeignKey(nameof(JobCategoryId))]
    public DotNetSkeleton.Modules.JobModule.Entities.JobCategoryEntity? JobCategory { get; set; }
}
