using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.ProfileModule.Entities;

[Table("profile_identities")]
public sealed class ProfileIdentityEntity
{
    [Key]
    [Column("profile_id")]
    public int ProfileId { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public ProfileEntity Profile { get; set; } = null!;

    [Required]
    [Column("card_id")]
    public string CardId { get; set; } = string.Empty;

    [Required]
    [Column("card_front_image")]
    public string CardFrontImage { get; set; } = string.Empty;

    [Required]
    [Column("card_back_image")]
    public string CardBackImage { get; set; } = string.Empty;

    [Required]
    [Column("full_name_on_card")]
    public string FullNameOnCard { get; set; } = string.Empty;
}
