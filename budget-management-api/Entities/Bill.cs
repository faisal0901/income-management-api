using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_management_api.Entities
{
    [Table("m_bill")]
    public class Bill
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("bill_name")]
        public string BillName { get; set; } = null;

        [Column("bill_date")]
        public DateTime BillDate { get; set; }

        [Column("bill_amount")]
        public long BillAmount { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        public virtual User? User { get; set; }
    }
}