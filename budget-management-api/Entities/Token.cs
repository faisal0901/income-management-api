using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget_management_api.Entities;
[Table("token")]
public class Token
{
    [Key,Column(name:"id")]
    public int  Id { get; set; }
    public string TokenValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte Revoked { get; set; }
    [Column(name: "user_id")] public Guid UserId { get; set; }
    public virtual User? User { get; set; }
}