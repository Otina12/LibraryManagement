using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Publisher;

namespace Library.Service.Dtos.Book;

public record BookDetailsDto(
    Guid Id,
    string ISBN,
    string Title,
    int Edition,
    int PageCount,
    string Description,
    int Quantity,
    int PublishYear
    )
{
    public Genre[] Genres { get; set; } = [];
    public AuthorIdAndNameDto[] AuthorsDto { get; set; } = [];
    public PublisherIdAndNameDto? PublisherDto { get; set; }
    public BookLocationDto[] Locations { get; set; } = [];
}

