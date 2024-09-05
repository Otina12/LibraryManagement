using Library.Model.Models;
using Library.Service.Dtos.BookCopy.Get;

namespace Library.Service.Dtos.BookCopyLog.Post;

public record CreateBookCopyLogDto(
    BookCopyDto BookCopyDto,
    BookCopyAction BookCopyAction,
    string? Comment,
    string? CustomerId = ""
    );
