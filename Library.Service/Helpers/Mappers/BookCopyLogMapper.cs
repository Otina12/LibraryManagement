using Library.Model.Models;
using Library.Service.Dtos.BookCopyLog.Post;

namespace Library.Service.Helpers.Mappers;

public static class BookCopyLogMapper
{
    public static BookCopyLog MapToBookCopyLog(this CreateBookCopyLogDto bookCopyLogDto)
    {
        return new BookCopyLog
        {
            BookCopyId = bookCopyLogDto.BookCopyDto.Id,
            BookCopyAction = bookCopyLogDto.BookCopyAction,
            CurrentStatus = bookCopyLogDto.BookCopyDto.Status,
            Comment = bookCopyLogDto.Comment ?? "",
            CustomerId = bookCopyLogDto.CustomerId == "" ? null : bookCopyLogDto.CustomerId,
            ActionTimeStamp = DateTime.UtcNow
        };
    }
}
