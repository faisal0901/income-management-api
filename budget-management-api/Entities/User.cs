using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using budget_management_api.Enum;

namespace budget_management_api.Entities;
[Table(name:"m_user")]
public class User
{
    [Key,Column(name:"id")]
    public Guid Id { get; set; }

    [Column(name: "email"), Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Column(name: "password"), Required, StringLength(maximumLength: int.MaxValue, MinimumLength = 6)]
    public string Password { get; set; } = null;
    [Column(name: "phone_number"), Required]
    public string PhoneNumber { get; set; } = null;
    [Column(name: "Role"), Required]
    public ERole Role { get; set; }
    
    public ICollection<Transactional>? Transactionals { get; set; } = null;
    public ICollection<Bill>? Bills { get; set; }= null;
    public ICollection<Wallet>? Wallets { get; set; }= null;
    public ICollection<Token>? Tokens { get; set; }= null;

}