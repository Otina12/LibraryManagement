using Library.Service.Dtos.Book.Get;

namespace Library.Service.Dtos.OriginalBook.Post;

public record EditOriginalBookDto(
    Guid Id,
    int OriginalPublishYear,
    DateTime CreationDate,
    bool isDeleted)
{
    public string EnglishTitle { get; set; } = string.Empty;
    public string? EnglishDescription { get; set; }
    public string GermanTitle { get; set; } = string.Empty;
    public string? GermanDescription { get; set; }
    public string GeorgianTitle { get; set; } = string.Empty;
    public string? GeorgianDescription { get; set; }
    public BookIdAndTitleDto[] Books { get; set; } = [];
    public List<int> GenreIds { get; set; } = [];
}
