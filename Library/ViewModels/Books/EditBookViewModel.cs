using Library.Service.Dtos.Book.Get;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Books;

public class EditBookViewModel
{
    public Guid Id { get; set; }

    [MaxLength(13, ErrorMessage = "ISBN must be at most 13 characters long")]
    public string ISBN { get; set; }
    public string Title { get; set; }
    public int Edition { get; set; }
    public int PageCount { get; set; }
    public string Description { get; set; }
    public int PublishYear { get; set; }

    [Display(Name = "Genres")]
    public List<int> GenreIds { get; set; } = [];

    [Display(Name = "Publisher")]
    public Guid? PublisherId { get; set; }

    [Display(Name = "Authors")]
    public List<Guid> AuthorIds { get; set; } = [];

    public BookLocationDto[] Locations { get; set; } = [];
}
