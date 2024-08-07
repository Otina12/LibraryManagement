﻿using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Book.Post;
using Library.Service.Dtos.Publisher.Get;
using Library.Service.Helpers;
using Library.Service.Helpers.Books;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;
using System.Linq.Expressions;
using Library.Service.Dtos.OriginalBook.Get;
using Library.Service.Dtos.BookCopy.Post;
using Library.Service.Dtos.BookCopyLog.Post;
using Library.Service.Helpers.Mappers;

namespace Library.Service.Services;

public class BookService : BaseService<Book>, IBookService
{

    public BookService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork, validationService)
    {
    }

    public async Task<Dictionary<OriginalBookIdAndTitle, IEnumerable<BookEditionDto>>> GetAllBookEditions(bool includeDeleted = false)
    {
        var dict = new Dictionary<OriginalBookIdAndTitle, IEnumerable<BookEditionDto>>();
        var originalBooks = _unitOfWork.OriginalBooks
            .GetAllAsQueryable()
            .OrderBy(x => x.Title)
            .ToList();
        
        foreach (var originalBook in originalBooks)
        {
            var originalBookDto = new OriginalBookIdAndTitle(originalBook.Id, originalBook.Title);
            var books = await _unitOfWork.Books.GetAllBookEditionsOfOriginalBook(originalBook.Id); // get all editions of that original book

            var bookDtos = books.Select(b => b.MapToBookEditionDto()).ToList();
            foreach(var bookDto in bookDtos)
            {
                bookDto.AvailableQuantity = await _unitOfWork.BookCopies.GetCountOfAvailableBookCopies(bookDto.Id);
            }

            // key - original book, value - all editions of that book
            dict.Add(originalBookDto, bookDtos);
        }

        return dict;
    }

    public async Task<EntityFiltersDto<BookDto>> GetAllFilteredBooks(EntityFiltersDto<BookDto> bookFilters)
    {
        var books = _unitOfWork.Books.GetAllAsQueryable();

        books = books.IncludeDeleted(bookFilters.IncludeDeleted);
        books = books.ApplySearch(bookFilters.SearchString, GetBookSearchProperties());
        bookFilters.TotalItems = books.Count();
        books = books.ApplySort(bookFilters.SortBy, bookFilters.SortOrder, GetBookSortDictionary());
        var finalBooks = books.ApplyPagination(bookFilters.PageNumber, bookFilters.PageSize).ToList();

        var booksDto = finalBooks.Select(b => b.MapToBookDto()).ToList();

        foreach (var bookDto in booksDto)
        {
            bookDto.AuthorsDto = await GetAuthorsOfABook(bookDto.Id);
            bookDto.PublisherDto = await GetPublisherOfABook(bookDto.Id);
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

        var book = bookExistsResult.Value();
        var bookDetailsDto = book.MapToBookDetailsDto();

        bookDetailsDto.PublisherDto = await GetPublisherOfABook(id);
        bookDetailsDto.AuthorsDto = await GetAuthorsOfABook(id);
        bookDetailsDto.Genres = await GetGenresOfABook((Guid)book.OriginalBookId!);
        bookDetailsDto.Locations = await GetLocationsOfABook(id);

        return bookDetailsDto;
    }

    public async Task<Result> CreateBook(CreateBookDto bookDto, string creationComment)
    {
        var bookIsNewResult = await _validationService.BookIsNew(bookDto.ISBN);

        if (bookIsNewResult.IsFailure) // book already exists
        {
            return bookIsNewResult.Error;
        }

        var originalBook = await _unitOfWork.OriginalBooks.GetById(bookDto.SelectedOriginalBookId);
        if (originalBook is null || bookDto.PublishYear < originalBook!.OriginalPublishYear)
        {
            return Result.Failure("Book.InvalidPublishYear", "Publish Year can't be less than Original Book's publish year");
        }

        // otherwise add the book
        var book = bookDto.MapToBook();
        book.AddAuthorsToBook(bookDto.SelectedAuthorIds);
        await _unitOfWork.Books.Create(book);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> UpdateBook(EditBookDto bookDto, string creationComment)
    {
        var bookExistsResult = await _validationService.BookExists(bookDto.Id);

        if (bookExistsResult.IsFailure)
        {
            return bookExistsResult.Error;
        }

        var book = bookExistsResult.Value();

        await _unitOfWork.Books.UpdateAuthorsForBook(book.Id, bookDto.AuthorIds);
        book.UpdatePublisher(bookDto.PublisherId);

        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
    
    public async Task<Result> CreateBookCopies(CreateBookCopiesDto bookCopiesDto)
    {
        var bookExistsResult = await _validationService.BookExists(bookCopiesDto.BookId, true);

        if (bookExistsResult.IsFailure)
        {
            return bookExistsResult.Error;
        }

        var book = bookExistsResult.Value();

        foreach (var location in bookCopiesDto.Locations) // creating book copies and logs for each copy location
        {
            var bookCopies = _unitOfWork.BookCopies.AddXBookCopies(book.Id, location.RoomId, location.ShelfId, location.Quantity, location.Status, bookCopiesDto.CreationComment ?? "");

            var bookCopiesLogsDto = bookCopies.Select(x => new CreateBookCopyLogDto(x.MapToBookCopyDto(), BookCopyAction.Created, bookCopiesDto.CreationComment));
            await _unitOfWork.BookCopyLogs.CreateRange(bookCopiesLogsDto.Select(x => x.MapToBookCopyLog()).ToList());
        }

        book.Quantity += bookCopiesDto.Locations.Select(x => x.Quantity).Sum();
        await _unitOfWork.SaveChangesAsync();

        // in case it was a 'Return Another Copy' option, we change original book copy's status from 'Lost' to 'LostAndReturnedAnotherCopy'
        if (bookCopiesDto.ReservationCopyId is not null)
            await UpdateReservationCopyStatus(bookCopiesDto.ReservationCopyId.Value);

        return Result.Success();
    }

    private async Task UpdateReservationCopyStatus(Guid reservationCopyId)
    {
        var reservationCopy = await _unitOfWork.ReservationCopies.GetById(reservationCopyId);
        if (reservationCopy == null) return;

        reservationCopy.ReturnedStatus = Model.Enums.BookCopyStatus.LostAndReturnedAnotherCopy;
        await _unitOfWork.SaveChangesAsync();
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

    private async Task<Genre[]> GetGenresOfABook(Guid originalBookId)
    {
        return (await _unitOfWork.Genres.GetAllGenresOfABook(originalBookId)).ToArray();
    }

    private async Task<BookLocationDto[]> GetLocationsOfABook(Guid bookId)
    {
        var locationValueObjects = await _unitOfWork.BookCopies.GetAllLocationsOfABook(bookId);
        return locationValueObjects
            .Select(x => new BookLocationDto(x.RoomId, x.ShelfId, x.Status, x.Quantity))
            .ToArray();
    }

    // Returns a dictionary that we will later use in generic sort method
    private static Dictionary<string, Expression<Func<Book, object>>> GetBookSortDictionary()
    {
        var dict = new Dictionary<string, Expression<Func<Book, object>>>
        {
            ["Title"] = b => b.OriginalBook.Title,
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
            b => b.OriginalBook.Title,
            b => b.ISBN
        ];
    }
}