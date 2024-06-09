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

    public async Task<IEnumerable<BookDto>> GetAllBooks()
    {
        var books = await _unitOfWork.Books.GetAll();
        var booksDto = books.Select(b => b.MapToBookDto()).ToList();

        foreach(var bookDto in booksDto)
        {
            await MapPublisherAndAuthors(bookDto);
        }

        return booksDto;
    }

    public async Task<Result<BookDto>> GetBookById(Guid id)
    {
        var bookExistsResult = await _validationService.BookExists(id);

        if (bookExistsResult.IsFailure)
        {
            return Result.Failure<BookDto>(bookExistsResult.Error);
        }

        var bookDto = bookExistsResult.Value().MapToBookDto();

        await MapPublisherAndAuthors(bookDto);

        return bookDto; // implicit casting to Result<BookDto> object
    }

    private async Task MapPublisherAndAuthors(BookDto bookDto)
    {
        var authors = await _unitOfWork.Authors.GetAuthorsOfABook(bookDto.Id);
        var publisher = await _unitOfWork.Publishers.GetPublisherOfABook(bookDto.Id);

        bookDto.AuthorsDto = authors.Select(a => new AuthorIdAndNameDto(a.Id, $"{a.Name} {a.Surname}")).ToArray();
        bookDto.PublisherDto = publisher is null ? null : new PublisherIdAndNameDto(publisher.Id, publisher.Name);
    }
}
