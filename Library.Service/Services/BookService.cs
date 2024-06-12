using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
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

    public async Task<Result> CreateBook(CreateBookDto bookDto)
    {
        var locations = DecodeBookLocations(bookDto.EncodedLocationsString);

        var book = bookDto.MapToBook();
        book.Quantity = locations.Sum(x => x.Quantity);

        await _unitOfWork.Books.Create(book);

        foreach (var authorId in bookDto.SelectedAuthorIds)
        {
            book.BookAuthors.Add(new BookAuthor { BookId = book.Id, AuthorId = authorId });
        }

        var bookCopies = CreateBookCopies(book.Id, locations);
        _unitOfWork.BookCopies.CreateRange(bookCopies);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public IEnumerable<BookCopy> CreateBookCopies(Guid bookId, IEnumerable<BookLocationDto> locations)
    {
        var bookCopies = new List<BookCopy>();
        foreach (var (room, shelf, quantity) in locations)
        {
            bookCopies.AddRange(Enumerable.Range(0, quantity)
                                        .Select(_ => new BookCopy
                                        {
                                            Status = Model.Enums.Status.Normal,
                                            BookId = bookId,
                                            RoomId = room,
                                            ShelfId = shelf,
                                            CreationDate = DateTime.UtcNow
                                        }));
        }
        return bookCopies;
    }

    private static List<BookLocationDto> DecodeBookLocations(string? encodedLocations)
    {
        if (encodedLocations is null)
        {
            return [];
        }

        return encodedLocations
            .Split(',')
            .Select(location =>
            {
                int[] info = location.Split('|').Select(x => int.Parse(x)).ToArray();
                return new BookLocationDto(RoomId: info[0], ShelfId: info[1], Quantity: info[2]);
            })
            .ToList();
    }

   
}
