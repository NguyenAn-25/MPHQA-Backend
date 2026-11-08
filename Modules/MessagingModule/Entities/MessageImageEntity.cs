using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.MessagingModule.Entities;

[Table("message_images")]
public sealed class MessageImageEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("image")]
    public string Image { get; set; } = string.Empty;

    [Required]
    [Column("message_id")]
    public int MessageId { get; set; }

    [ForeignKey(nameof(MessageId))]
    public MessageEntity Message { get; set; } = null!;
}
