using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using budget_management_api.Enum;

namespace budget_management_api.Entities;
[Table(name:"wallet_user")]
public class Wallet
{
    [Key,Column(name:"id")]
    public Guid Id { get; set; }
    [Column(name:"wallet_balance")]
    public long WalletBalance { get; set; }
    [Column(name:"wallet_type")]
    public EWalletType WalletType { get; set; }
    [Column(name: "user_id")] public Guid UserId { get; set; }
    public virtual User? User { get; set; }
    public ICollection<Gold>? Golds{ get; set; } = null;
}