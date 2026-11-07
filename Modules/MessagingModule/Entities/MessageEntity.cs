using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.MessagingModule.Entities;

[Table("messages")]
public sealed class MessageEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("content", TypeName = "text")]
    public string? Content { get; set; }

    [Column("reply_to_message_id")]
    public int? ReplyToMessageId { get; set; }

    [ForeignKey(nameof(ReplyToMessageId))]
    public MessageEntity? ReplyToMessage { get; set; }

    public ICollection<MessageEntity> Replies { get; set; } = new List<MessageEntity>();

    [Required]
    [Column("sender_id")]
    public int SenderId { get; set; }

    [ForeignKey(nameof(SenderId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity Sender { get; set; } = null!;

    [Required]
    [Column("conversation_id")]
    public int ConversationId { get; set; }

    [ForeignKey(nameof(ConversationId))]
    public ConversationEntity Conversation { get; set; } = null!;

    public ICollection<MessageImageEntity> Images { get; set; } = new List<MessageImageEntity>();
}
