using Library.Service.Dtos.Book.Get;

namespace Library.Service.Dtos.OriginalBook.Get;

public record OriginalBookDto(
    Guid Id,
    int OriginalPublishYear,
    DateTime CreationDate,
    bool isDeleted)
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public BookIdAndTitleDto[] Books { get; set; } = [];
    public List<int> GenreIds { get; set; } = [];
}
