using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.WalletModule.Entities;

[Table("topup_orders")]
public sealed class TopUpOrderEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("coin_amount")]
    public int CoinAmount { get; set; }

    [Required]
    [Column("money_amount", TypeName = "numeric(15,2)")]
    public decimal MoneyAmount { get; set; }

    [Required]
    [Column("order_code")]
    public string OrderCode { get; set; } = string.Empty;

    [Column("paid_at")]
    public DateTimeOffset? PaidAt { get; set; }

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [Column("status")]
    public string Status { get; set; } = "PENDING";

    [Column("transaction_id")]
    public string? TransactionId { get; set; }

    [Required]
    [Column("wallet_id")]
    public int WalletId { get; set; }

    [ForeignKey(nameof(WalletId))]
    public WalletEntity Wallet { get; set; } = null!;

    [Required]
    [Column("payment_method_id")]
    public int PaymentMethodId { get; set; }

    [ForeignKey(nameof(PaymentMethodId))]
    public PaymentMethodEntity PaymentMethod { get; set; } = null!;
}
