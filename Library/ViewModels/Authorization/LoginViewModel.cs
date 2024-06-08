using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Authorization;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }


}
