using Library.Service.Dtos.Book.Get;

namespace Library.Service.Dtos.BookCopy.Post;

public record CreateBookCopiesDto(
    Guid BookId,
    string? CreationComment,
    BookLocationDto[] Locations,
    Guid? ReservationCopyId
    );
