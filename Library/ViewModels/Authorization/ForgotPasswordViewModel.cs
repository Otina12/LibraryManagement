using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Authorization;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string Email { get; set; }
}
