using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Book.Post;
using Library.Service.Dtos.Publisher.Get;
using Library.Service.Helpers;
using Library.Service.Helpers.Books;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Service.Services;

public class BookService : BaseService<Book>, IBookService
{
    public BookService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork, validationService)
    {
    }

    public async Task<IEnumerable<BookIdTitleAndQuantityDto>> GetAllBooksSorted()
    {
        var books = await _unitOfWork.Books
            .GetAllAsQueryable()
            .OrderBy(x => x.Title)
            .ToListAsync();

        return books.Select(b => new BookIdTitleAndQuantityDto(b.Id, b.Title, b.Quantity));
    }

    public async Task<EntityFiltersDto<BookDto>> GetAllFilteredBooks(EntityFiltersDto<BookDto> bookFilters)
    {
        var books = _unitOfWork.Books.GetAllAsQueryable();

        books = books.IncludeDeleted(bookFilters.IncludeDeleted);
        books = books.ApplySearch(bookFilters.SearchString, GetBookSearchProperties());
        bookFilters.TotalItems = await books.CountAsync();
        books = books.ApplySort(bookFilters.SortBy, bookFilters.SortOrder, GetBookSortDictionary());
        books = books.ApplyPagination(bookFilters.PageNumber, bookFilters.PageSize);

        var booksDto = await books.Select(b => b.MapToBookDto()).ToListAsync();

        foreach (var bookDto in booksDto)
        {
            bookDto.PublisherDto = await GetPublisherOfABook(bookDto.Id);
            bookDto.AuthorsDto = await GetAuthorsOfABook(bookDto.Id);
        }

        bookFilters.Entities = booksDto;
        return bookFilters;
    }

    public async Task<Result<BookDetailsDto>> GetBookById(Guid id)
    {
        var bookExistsResult = await _validationService.BookExists(id);

        if (bookExistsResult.IsFailure)
        {
            return Result.Failure<BookDetailsDto>(bookExistsResult.Error);
        }

        var bookDetailsDto = bookExistsResult.Value().MapToBookDetailsDto();

        bookDetailsDto.PublisherDto = await GetPublisherOfABook(id);
        bookDetailsDto.AuthorsDto = await GetAuthorsOfABook(id);
        bookDetailsDto.Genres = await GetGenresOfABook(id);
        bookDetailsDto.Locations = await GetLocationsOfABook(id);

        return bookDetailsDto;
    }

    public async Task<Result> CreateBook(CreateBookDto bookDto)
    {
        var bookIsNewResult = await _validationService.BookIsNew(bookDto.ISBN);

        if (bookIsNewResult.IsFailure) // book already exists
        {
            return bookIsNewResult.Error;
        }

        var book = bookDto.MapToBook();
        book.Quantity = bookDto.Locations.Sum(x => x.Quantity);
        book.AddAuthorsToBook(bookDto.SelectedAuthorIds);
        book.AddGenresToBook(bookDto.SelectedGenreIds);

        await _unitOfWork.Books.Create(book);
        CreateBookCopies(book.Id, bookDto.Locations); // create 'quantity' copies of the book

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateBook(EditBookDto bookDto)
    {
        var bookExistsResult = await _validationService.BookExists(bookDto.Id);

        if (bookExistsResult.IsFailure)
        {
            return bookExistsResult.Error;
        }

        var book = bookExistsResult.Value();

        await _unitOfWork.Books.UpdateGenresForBook(book.Id, bookDto.GenreIds);
        await _unitOfWork.Books.UpdateAuthorsForBook(book.Id, bookDto.AuthorIds);
        book.UpdatePublisher(bookDto.PublisherId);

        var (locationsToRemove, locationsToAdd, count) = book.UpdateLocations(await GetLocationsOfABook(book.Id), bookDto.Locations);
        DeleteBookCopies(book.Id, locationsToRemove);
        CreateBookCopies(book.Id, locationsToAdd);
        book.Quantity += count;

        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    // helpers
    private void CreateBookCopies(Guid bookId, IEnumerable<BookLocationDto> locations)
    {
        foreach (var location in locations)
        {
            _unitOfWork.BookCopies.AddXBookCopies(bookId, location.RoomId, location.ShelfId, location.Quantity);
        }
    }

    private void DeleteBookCopies(Guid bookId, IEnumerable<BookLocationDto> locations)
    {
        foreach (var location in locations)
        {
            _unitOfWork.BookCopies.DeleteXBookCopies(bookId, location.RoomId, location.ShelfId, location.Quantity);
        }
    }

    // book GET methods (helpers)
    private async Task<PublisherIdAndNameDto?> GetPublisherOfABook(Guid bookId)
    {
        var publisher = await _unitOfWork.Publishers.GetPublisherOfABook(bookId);
        return publisher is null ? null : new PublisherIdAndNameDto(publisher.Id, publisher.Name);
    }

    private async Task<AuthorIdAndNameDto[]> GetAuthorsOfABook(Guid bookId)
    {
        var authors = await _unitOfWork.Authors.GetAuthorsOfABook(bookId);
        return authors.Select(a => new AuthorIdAndNameDto(a.Id, $"{a.Name} {a.Surname}")).ToArray();
    }

    private async Task<Genre[]> GetGenresOfABook(Guid bookId)
    {
        return (await _unitOfWork.Genres.GetAllGenresOfABook(bookId)).ToArray();
    }

    private async Task<BookLocationDto[]> GetLocationsOfABook(Guid bookId)
    {
        var locationValueObjects = await _unitOfWork.BookCopies.GetAllLocationsOfABook(bookId);
        return locationValueObjects
            .Select(x => new BookLocationDto(x.RoomId, x.ShelfId, x.Quantity))
            .ToArray();
    }

    // Returns a dictionary that we will later use in generic sort method
    private static Dictionary<string, Expression<Func<Book, object>>> GetBookSortDictionary()
    {
        var dict = new Dictionary<string, Expression<Func<Book, object>>>
        {
            ["Title"] = b => b.Title,
            ["PublishYear"] = b => b.PublishYear,
            ["Quantity"] = b => b.Quantity
        };

        return dict;
    }

    // Returns a function that we will use to search items
    private static Func<Book, string>[] GetBookSearchProperties()
    {
        return
        [
            b => b.Title,
            b => b.ISBN
        ];
    }

}

