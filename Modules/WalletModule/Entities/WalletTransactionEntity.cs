using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.WalletModule.Entities;

[Table("wallet_transactions")]
public sealed class WalletTransactionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("coin_amount")]
    public int CoinAmount { get; set; }

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [Column("balance_after")]
    public int BalanceAfter { get; set; }

    [Column("object_type")]
    public string? ObjectType { get; set; }

    [Column("object_id")]
    public int? ObjectId { get; set; }

    [Required]
    [Column("type")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [Column("wallet_id")]
    public int WalletId { get; set; }

    [ForeignKey(nameof(WalletId))]
    public WalletEntity Wallet { get; set; } = null!;
}
