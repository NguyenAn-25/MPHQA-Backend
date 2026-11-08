using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetSkeleton.Modules.WalletModule.Entities;

[Table("payment_methods")]
public sealed class PaymentMethodEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("icon")]
    public string? Icon { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    public ICollection<TopUpOrderEntity> TopUpOrders { get; set; } = new List<TopUpOrderEntity>();
}
