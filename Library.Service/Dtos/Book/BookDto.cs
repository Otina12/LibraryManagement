using Library.Service.Dtos.Author;
using Library.Service.Dtos.Publisher;

namespace Library.Service.Dtos.Book;

public record BookDto(
    Guid Id,
    string ISBN,
    string Title,
    int Edition,
    int PublishYear,
    int Quantity
    )
{
    public AuthorIdAndNameDto[] AuthorsDto { get; set; } = [];
    public PublisherIdAndNameDto? PublisherDto { get; set; }
}
   
