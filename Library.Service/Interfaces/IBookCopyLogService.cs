using Library.Model.Abstractions;
using Library.Model.Models;

namespace Library.Service.Interfaces;

public interface IBookCopyLogService
{
    Task<Result<IEnumerable<BookCopyLog>>> GetAllLogsOfBookCopy(Guid Id);
    Task CreateBookCopyLog(BookCopy bookCopy, BookCopyAction action, string? comment);
    Task CreateBookCopyLogs(IEnumerable<BookCopy> bookCopies, BookCopyAction action, string? comment);
}
