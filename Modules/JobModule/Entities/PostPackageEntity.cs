using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.JobModule.Entities;

[Table("post_packages")]
public sealed class PostPackageEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("duration_days")]
    public int DurationDays { get; set; }

    [Required]
    [Column("price_coin")]
    public int PriceCoin { get; set; }

    [Required]
    [Column("price_money", TypeName = "numeric(15,2)")]
    public decimal PriceMoney { get; set; }

    public ICollection<JobEntity> Jobs { get; set; } = new List<JobEntity>();
}
