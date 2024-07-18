using Library.Service.Dtos.Book.Get;

namespace Library.Service.Dtos.Publisher.Get;

public record PublisherDto(
    Guid Id,
    string Name,
    string? Email,
    string? PhoneNumber,
    int YearPublished,
    DateTime CreationDate,
    bool isDeleted
    )
{
    public BookIdAndTitleDto[] Books { get; set; } = [];
}