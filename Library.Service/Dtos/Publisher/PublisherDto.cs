using Library.Service.Dtos.Book;

namespace Library.Service.Dtos.Publisher;

public record PublisherDto(
    Guid Id,
    string Name,
    string? Email,
    string? PhoneNumber,
    int YearPublished,
    DateTime CreationDate
    )
{
    public BookIdAndTitleDto[] Books { get; set; } = [];
}