using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_management_api.Entities;
[Table("m_bill", Schema = "dbo")]
public class Bill
{
    [Key,Column(name:"id")]
    public Guid Id { get; set; }
    [Column(name: "bill_name")]
    public string BillName { get; set; } = null;
    [Column(name: "bill_date")] public DateTime BillDate { get; set; }
    [Column(name: "bill_ammount")] public long BillAmount { get; set; }
    [Column(name: "user_id")] public Guid UserId { get; set; }
    public virtual User? User { get; set; }
}