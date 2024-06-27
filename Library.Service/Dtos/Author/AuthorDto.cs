using Library.Service.Dtos.Book;

namespace Library.Service.Dtos.Author;

public record AuthorDto(
    Guid Id,
    string Name,
    string Surname,
    string? Email,
    string Description,
    int BirthYear,
    int? DeathYear,
    DateTime CreationDate
)
{
    public BookIdAndTitleDto[] Books { get; set; } = [];
}
