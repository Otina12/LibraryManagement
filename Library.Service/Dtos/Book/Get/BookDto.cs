using Library.Service.Dtos.Author;
using Library.Service.Dtos.Publisher.Get;

namespace Library.Service.Dtos.Book.Get;

public record BookDto(
    Guid Id,
    string ISBN,
    string Title,
    int Edition,
    int PublishYear,
    int Quantity,
    bool isDeleted
    )
{
    public AuthorIdAndNameDto[] AuthorsDto { get; set; } = [];
    public PublisherIdAndNameDto? PublisherDto { get; set; }
}

