using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.WalletModule.Entities;

[Table("wallets")]
public sealed class WalletEntity
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public DotNetSkeleton.Modules.AuthModule.Entities.UserEntity User { get; set; } = null!;

    [Required]
    [Column("balance")]
    public int Balance { get; set; }

    public ICollection<WalletTransactionEntity> Transactions { get; set; } = new List<WalletTransactionEntity>();
    public ICollection<TopUpOrderEntity> TopUpOrders { get; set; } = new List<TopUpOrderEntity>();
}
