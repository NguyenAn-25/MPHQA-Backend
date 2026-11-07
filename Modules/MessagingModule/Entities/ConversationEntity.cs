using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.MessagingModule.Entities;

[Table("conversations")]
public sealed class ConversationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_a_id")]
    public int UserAId { get; set; }

    [ForeignKey(nameof(UserAId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity UserA { get; set; } = null!;

    [Required]
    [Column("user_b_id")]
    public int UserBId { get; set; }

    [ForeignKey(nameof(UserBId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity UserB { get; set; } = null!;

    public ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
}
