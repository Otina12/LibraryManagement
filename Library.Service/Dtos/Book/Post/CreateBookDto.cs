using Library.Service.Dtos.Book.Get;

namespace Library.Service.Dtos.Book.Post;

public record CreateBookDto(
    string ISBN,
    string Title,
    int Edition,
    int PageCount,
    string Description,
    int PublishYear,
    int[] SelectedGenreIds,
    Guid? SelectedPublisherId,
    Guid[] SelectedAuthorIds,
    BookLocationDto[] Locations
    );
