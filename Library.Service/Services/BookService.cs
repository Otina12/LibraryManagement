using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Publisher;
using Library.Service.Extensions;
using Library.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;

namespace Library.Service.Services;

public class BookService : BaseService<Book>, IBookService
{
    private readonly IValidationService _validationService;

    public BookService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork)
    {
        _validationService = validationService;
    }

    public async Task<BookListDto> GetAllFilteredBooks(BookListDto bookFilters)
    {
        var books = _unitOfWork.Books.GetAllAsQueryable();
        bookFilters.TotalItems = await books.CountAsync();

        books = ApplySearch(books, bookFilters.SearchString);
        books = ApplySort(books, bookFilters.SortBy, bookFilters.SortOrder);
        books = ApplyPagination(books, bookFilters.PageNumber, bookFilters.PageSize);

        var booksDto = await books.Select(b => b.MapToBookDto()).ToListAsync();

        foreach (var bookDto in booksDto)
        {
            await MapPublisher(bookDto);
            await MapAuthors(bookDto);
        }

        bookFilters.Books = booksDto;

        return bookFilters;
    }

    public async Task<Result<BookDto>> GetBookById(Guid id)
    {
        var bookExistsResult = await _validationService.BookExists(id);

        if (bookExistsResult.IsFailure)
        {
            return Result.Failure<BookDto>(bookExistsResult.Error);
        }

        var bookDto = bookExistsResult.Value().MapToBookDto();

        await MapPublisher(bookDto);
        await MapAuthors(bookDto);

        return bookDto; // implicit casting to Result<BookDto> object
    }

    public async Task<Result> CreateBook(CreateBookDto bookDto)
    {
        var bookId = Guid.Empty;
        var quantity = bookDto.Locations.Sum(x => x.Quantity);

        var bookExistsResult = await _validationService.BookExists(bookDto.ISBN);
        
        if (bookExistsResult.IsSuccess) // book exists so we only need to add copies
        {
            var book = bookExistsResult.Value();
            bookId = book.Id;
            book.Quantity += quantity;
            book.UpdateDate = DateTime.UtcNow;
            _unitOfWork.Books.Update(book);
        }
        else // book does not exist so we add it, set quantity
        {
            var book = bookDto.MapToBook();
            book.Quantity = quantity;
            AddAuthorsToBook(book, bookDto.SelectedAuthorIds);
            await _unitOfWork.Books.Create(book);
            bookId = book.Id;
        }

        var bookCopies = CreateBookCopies(bookId, bookDto.Locations);
        _unitOfWork.BookCopies.CreateRange(bookCopies);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public IEnumerable<BookCopy> CreateBookCopies(Guid bookId, IEnumerable<BookLocationDto> locations)
    {
        var bookCopies = new List<BookCopy>();

        foreach (var location in locations)
        {
            bookCopies.AddRange(Enumerable.Range(0, location.Quantity)
                                        .Select(_ => new BookCopy
                                        {
                                            Status = Model.Enums.Status.Normal,
                                            BookId = bookId,
                                            RoomId = location.RoomId,
                                            ShelfId = location.ShelfId,
                                            CreationDate = DateTime.UtcNow
                                        }));
        }

        return bookCopies;
    }


    private async Task MapPublisher(BookDto bookDto)
    {
        var publisher = await _unitOfWork.Publishers.GetPublisherOfABook(bookDto.Id);
        bookDto.PublisherDto = publisher is null ? null : new PublisherIdAndNameDto(publisher.Id, publisher.Name);
    }

    private async Task MapAuthors(BookDto bookDto)
    {
        var authors = await _unitOfWork.Authors.GetAuthorsOfABook(bookDto.Id);
        bookDto.AuthorsDto = authors.Select(a => new AuthorIdAndNameDto(a.Id, $"{a.Name} {a.Surname}")).ToArray();
    }

    private static void AddAuthorsToBook(Book book, Guid[] authorIds)
    {
        foreach(var authorId in authorIds)
        {
            book.BookAuthors.Add(new BookAuthor { BookId = book.Id, AuthorId = authorId });
        }
    }

    // search and sorts
    private static IQueryable<Book> ApplySearch(IQueryable<Book> books, string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            string searchLower = searchString.ToLower();
            return books.Where(b => EF.Functions.Like(b.Title.ToLower(), $"%{searchLower}%") ||
                                    EF.Functions.Like(b.ISBN.ToLower(), $"%{searchLower}%"));
        }
        return books;
    }

    private static IQueryable<Book> ApplySort(IQueryable<Book> books, string sortBy, string sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy) || string.IsNullOrEmpty(sortOrder))
        {
            return books;
        }

        return (sortBy.ToLower(), sortOrder) switch
        {
            ("title", "asc") => books.OrderBy(b => b.Title),
            ("title", "desc") => books.OrderByDescending(b => b.Title),
            ("year", "asc") => books.OrderBy(b => b.PublishYear),
            ("year", "desc") => books.OrderByDescending(b => b.PublishYear),
            ("quantity", "asc") => books.OrderBy(b => b.Quantity),
            ("quantity", "desc") => books.OrderByDescending(b => b.Quantity),
            _ => books
        };
    }

    private static IQueryable<Book> ApplyPagination(IQueryable<Book> books, int pageNumber, int pageSize)
    {
        return books.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}
