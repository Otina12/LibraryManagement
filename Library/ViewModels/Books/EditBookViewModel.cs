using Library.Service.Dtos.Book;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Books;

public class EditBookViewModel
{
    public Guid Id { get; set; }

    [MaxLength(13)]
    public string ISBN { get; set; }
    public string Title { get; set; }
    public int Edition { get; set; }
    public int PageCount { get; set; }
    public string Description { get; set; }
    public int PublishYear { get; set; }

    [Display(Name = "Genres")]
    public List<int> SelectedGenreIds { get; set; } = [];

    [Display(Name = "Publisher")]
    public Guid SelectedPublisherId { get; set; }

    [Display(Name = "Authors")]
    public List<Guid> SelectedAuthorIds { get; set; } = [];

    public List<BookLocationDto> Locations { get; set; } = [];
}
