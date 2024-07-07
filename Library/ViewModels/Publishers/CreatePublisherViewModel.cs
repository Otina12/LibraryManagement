using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Publishers;

public class CreatePublisherViewModel
{
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string? Email { get; set; }
    [MaxLength(25, ErrorMessage = "Phone number must be at most 25 characters long")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }
    public int YearPublished { get; set; }
}
