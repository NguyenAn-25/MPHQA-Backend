using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotNetSkeleton.Modules.ProfileModule.Entities;

[Table("profiles")]
[Index(nameof(UserId), IsUnique = true)]
public sealed class ProfileEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("note", TypeName = "text")]
    public string? Note { get; set; }

    [Column("major")]
    public string? Major { get; set; }

    [Column("gender")]
    public string? Gender { get; set; }

    [Column("avatar")]
    public string? Avatar { get; set; }

    [Required]
    [Column("fullname")]
    public string Fullname { get; set; } = string.Empty;

    [Column("birthdate", TypeName = "date")]
    public DateOnly? Birthdate { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("lng", TypeName = "numeric(10,7)")]
    public decimal? Longitude { get; set; }

    [Column("lat", TypeName = "numeric(10,7)")]
    public decimal? Latitude { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity User { get; set; } = null!;

    public ProfileIdentityEntity? Identity { get; set; }
    public ICollection<ProfileSkillEntity> ProfileSkills { get; set; } = new List<ProfileSkillEntity>();
    public ICollection<ProfileExperienceEntity> Experiences { get; set; } = new List<ProfileExperienceEntity>();
}
