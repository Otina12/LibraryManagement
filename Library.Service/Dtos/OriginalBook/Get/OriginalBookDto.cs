using Library.Service.Dtos.Book.Get;

namespace Library.Service.Dtos.OriginalBook.Get;

public record OriginalBookDto(
    Guid Id,
    string Title,
    string? Description,
    int OriginalPublishYear,
    DateTime CreationDate,
    bool isDeleted)
{
    public BookIdAndTitleDto[] Books { get; set; } = [];
}
