using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.JobModule.Entities;

[Table("job_categories")]
public sealed class JobCategoryEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Column("icon")]
    public string? Icon { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("parent_id")]
    public int? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public JobCategoryEntity? Parent { get; set; }

    public ICollection<JobCategoryEntity> Children { get; set; } = new List<JobCategoryEntity>();
    [NotMapped]
    public ICollection<JobEntity> Jobs { get; set; } = new List<JobEntity>();
}
