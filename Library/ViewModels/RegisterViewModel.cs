using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels;


public class RegisterViewModel
{
    public required string Name { get; set; }

    public required string Surname { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string Email { get; set; }


    [Required(ErrorMessage = "Username is required")]
    [MinLength(8, ErrorMessage = "Username must be at least 8 characters long")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Wrong phone number format")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }




}
