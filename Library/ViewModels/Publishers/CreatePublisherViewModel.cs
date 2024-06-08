using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Publishers;

public class CreatePublisherViewModel
{
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }
    public int YearPublished { get; set; }
}
