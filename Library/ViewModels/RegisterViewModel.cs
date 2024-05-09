using Library.ViewModels.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels;


public class RegisterViewModel
{
    public required string Name { get; set; }

    public required string Surname { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "Username is required")]
    [MinLength(8, ErrorMessage = "Username must be at least 8 characters long")]
    public string UserName { get; set; } = null!;

    // Date of Birth inputs

    [RangeYearToCurrent(1850, ErrorMessage = "Enter a valid year")]
    [Required(ErrorMessage = "Birth Year is required")]
    public int Year { get; set; }

    [Range(1, 12, ErrorMessage = "Enter a valid month (1-12)")]
    [Required(ErrorMessage = "Birth Month is required")]
    public int Month { get; set; }

    [Range(1, 31, ErrorMessage = "Enter a valid day")]
    [Required(ErrorMessage = "Birth Day is required")]
    public int Day { get; set; }


    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Wrong phone number format")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

}
