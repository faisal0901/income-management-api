using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_management_api.Entities;

[Table(name:"gold")]
public class Gold
{
    [Key,Column(name:"id")]
    public Guid Id { get; set; }
    [Column(name:"gram")]
    public int Gram { get;set; }
    [Column(name:"trans_date")]
    public DateTime TransDate { get; set; }
    [Column(name:"price")]
    public long Price { get; set; }
    [Column(name: "wallet_id")] 
    public Guid? WalletId { get; set; }
    public virtual Wallet Wallet { get; set; }
}