using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Authors;

public class CreateAuthorViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }

    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string? Email { get; set; }

    [MaxLength(800, ErrorMessage = "Description must be at most 800 characters long")]
    public string Description { get; set; }
    public int BirthYear { get; set; }
    public int? DeathYear { get; set; }
}
