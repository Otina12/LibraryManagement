using Library.Model.Models;
using Library.Service.Dtos.BookCopy.Get;

namespace Library.Service.Helpers.Mappers;

public static class BookCopyMapper
{
    public static BookCopyDto MapToBookCopyDto(this BookCopy bookCopy)
    {
        return new BookCopyDto(
            bookCopy.Id,
            bookCopy.Status,
            bookCopy.RoomId,
            bookCopy.ShelfId
            );
    }
}
