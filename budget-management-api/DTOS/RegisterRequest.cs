using System.ComponentModel.DataAnnotations;

namespace budget_management_api.Dtos;

public class RegisterRequest
{
    [Required, EmailAddress]public string Email { get; set; } = String.Empty;
    [Required, StringLength(maximumLength: int.MaxValue, MinimumLength = 6)]public string Password { get; set; } = String.Empty;
    [Required] public string PhoneNumber { get; set; } = String.Empty;
}
