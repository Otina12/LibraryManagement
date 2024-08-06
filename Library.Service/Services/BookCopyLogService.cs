using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.BookCopyLog.Post;
using Library.Service.Helpers.Mappers;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class BookCopyLogService : IBookCopyLogService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookCopyLogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<BookCopyLog>>> GetAllLogsOfBookCopy(Guid Id)
    {
        var bookCopy = await _unitOfWork.BookCopies.GetById(Id, false);
        if (bookCopy is null)
        {
            return Result.Failure(Error<IEnumerable<BookCopyLog>>.NotFound);
        }

        var logs = await _unitOfWork.BookCopyLogs.GetAllWhere(x => x.BookCopyId == Id);
        logs = logs.OrderBy(x => x.ActionTimeStamp).ToList();

        return Result.Success(logs);
    }

    public async Task CreateBookCopyLog(BookCopy bookCopy, BookCopyAction action, string? comment)
    {
        var bookCopyLogDto = new CreateBookCopyLogDto(bookCopy.MapToBookCopyDto(), action, comment);
        var bookCopyLog = bookCopyLogDto.MapToBookCopyLog();

        await _unitOfWork.BookCopyLogs.Create(bookCopyLog);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateBookCopyLogs(IEnumerable<BookCopy> bookCopies, BookCopyAction action, string? comment)
    {
        var bookCopyLogsDto = bookCopies
            .Select(x => new CreateBookCopyLogDto(x.MapToBookCopyDto(), action, comment))
            .ToList();

        var bookCopyLogs = bookCopyLogsDto
            .Select(x => x.MapToBookCopyLog())
            .ToList();

        await _unitOfWork.BookCopyLogs.CreateRange(bookCopyLogs);
        await _unitOfWork.SaveChangesAsync();
    }
}
