using Library.Service.Dtos.Book.Get;

namespace Library.Service.Dtos.Book.Post;

public record CreateBookDto(
    Guid SelectedOriginalBookId,
    string ISBN,
    int Edition,
    int PageCount,
    int PublishYear,
    int[] SelectedGenreIds,
    Guid? SelectedPublisherId,
    Guid[] SelectedAuthorIds,
    BookLocationDto[] Locations
    );
