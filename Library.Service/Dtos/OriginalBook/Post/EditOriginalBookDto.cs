using Library.Service.Dtos.Book.Get;

namespace Library.Service.Dtos.OriginalBook.Post;

public record EditOriginalBookDto(
    Guid Id,
    int OriginalPublishYear,
    DateTime CreationDate,
    bool isDeleted)
{
    public string EnglishTitle { get; set; } = string.Empty;
    public string EnglishDescription { get; set; } = string.Empty;
    public string GermanTitle { get; set; } = string.Empty;
    public string GermanDescription { get; set; } = string.Empty;
    public string GeorgianTitle { get; set; } = string.Empty;
    public string GeorgianDescription { get; set; } = string.Empty;
    public BookIdAndTitleDto[] Books { get; set; } = [];
    public List<int> GenreIds { get; set; } = [];

    public (string title, string description) GetTranslation(int languageId)
    {
        return languageId switch
        {
            1 => (EnglishTitle, EnglishDescription),
            2 => (GermanTitle, GermanDescription),
            3 => (GeorgianTitle, GeorgianDescription),
            _ => (string.Empty, string.Empty)
        };
    }
}
