using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Authors;

public class AuthorViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string? Email { get; set; }

    [MaxLength(1500)]
    public string Description { get; set; }
    public int BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public int BookCount { get; set; }
}
