namespace Library.Service.Dtos.Book;

public record CreateBookDto(
    string ISBN,
    string Title,
    int Edition,
    int PageCount,
    string Description,
    int PublishYear,
    Guid SelectedPublisherId,
    List<Guid> SelectedAuthorIds,
    string? EncodedLocationsString
    );
