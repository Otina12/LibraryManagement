namespace Library.Service.Dtos.Book;

public record EditBookDto(
    Guid Id,
    string ISBN,
    string Title,
    int Edition,
    int PageCount,
    string Description,
    int PublishYear,
    List<int> GenreIds,
    Guid? PublisherId,
    List<Guid> AuthorIds,
    BookLocationDto[] Locations
    );
