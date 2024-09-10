using Library.Service.Dtos.Book.Get;

namespace Library.ViewModels.OriginalBooks;

public class EditOriginalBookViewModel
{
    public Guid Id { get; set; }
    public string EnglishTitle { get; set; }
    public string? EnglishDescription { get; set; }
    public string GermanTitle { get; set; }
    public string? GermanDescription { get; set; }
    public string GeorgianTitle { get; set; }
    public string? GeorgianDescription { get; set; }
    public int OriginalPublishYear { get; set; }
    public DateTime CreationDate { get; set; }
    public bool isDeleted { get; set; }
    public BookIdAndTitleDto[] Books { get; set; } = [];
    public List<int> GenreIds { get; set; } = [];
}
