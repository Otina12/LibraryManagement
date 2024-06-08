using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Publisher;
using Library.Service.Extensions;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public BookService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }

    

    public Task<IEnumerable<BookDto>> GetAllBooks()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<BookDto>> GetBookById(Guid id)
    {
        var bookExistsResult = await _validationService.BookExists(id);

        if (bookExistsResult.IsFailure)
        {
            return Result.Failure<BookDto>(bookExistsResult.Error);
        }

        var bookDto = bookExistsResult.Value().MapToBookDto();

        var authors = await _unitOfWork.Authors.GetAuthorsOfABook(id);
        var publisher = await _unitOfWork.Publishers.GetPublisherOfABook(id);

        bookDto.AuthorsDto = authors.Select(a => new AuthorIdAndNameDto(a.Id, a.Name)).ToArray();
        bookDto.PublisherDto = publisher is null ? null : new PublisherIdAndNameDto(publisher.Id, publisher.Name);

        return bookDto; // implicit casting to Result<BookDto> object
    }
}
