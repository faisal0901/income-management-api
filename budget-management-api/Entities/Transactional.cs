using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using budget_management_api.Enum;

namespace budget_management_api.Entities;
[Table(name:"m_transactional")]
public class Transactional
{
    [Key,Column(name:"id")]
    public Guid Id { get; set; }
    [Column(name:"balance")]
    public long Balance { get; set; }
    
    [Column(name:"type")]
    public ETransactionType Type { get; set; }
    [Column(name:"trans_date")]
    public DateTime TransDate { get; set; }

    [Column(name: "user_id")] public Guid UserId { get; set; }
    [Column(name: "sub_category_id")] public Guid? SubCategoryId { get; set; }
    [Column(name: "wallet_id")] public Guid? WalletId { get; set; }
    public virtual SubCategory? SubCategory { get; set; } = null;
    public virtual User? User { get; set; }
    public virtual Wallet? Wallet { get; set; } = null;
}