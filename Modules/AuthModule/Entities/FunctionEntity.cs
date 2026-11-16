using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.AuthModule.Entities;

[Table("functions")]
[Index(nameof(Code), IsUnique = true)]
public sealed class FunctionEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("code")]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [Column("name_vi")]
    [MaxLength(100)]
    public string NameVi { get; set; } = string.Empty;

    [Required]
    [Column("name_en")]
    [MaxLength(100)]
    public string NameEn { get; set; } = string.Empty;

    [Required]
    [Column("actions", TypeName = "text[]")]
    public string[] Actions { get; set; } = [];

    public ICollection<PermissionDetailEntity> PermissionDetails { get; set; } = new List<PermissionDetailEntity>();
}
