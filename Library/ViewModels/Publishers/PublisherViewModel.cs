using Library.Service.Dtos.Book.Get;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Publishers;

public class PublisherViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    [MaxLength(25, ErrorMessage = "Phone number must be at most 25 characters long")]
    public string? PhoneNumber { get; set; }
    public int YearPublished { get; set; }
    public BookIdAndTitleDto[] Books { get; set; } = [];
    public DateTime CreationDate { get; set; }
    public bool isDeleted { get; set; }
}
