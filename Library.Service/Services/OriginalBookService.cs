using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Report;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.OriginalBook.Get;
using Library.Service.Dtos.OriginalBook.Post;
using Library.Service.Dtos.Report;
using Library.Service.Helpers;
using Library.Service.Helpers.Mappers;
using Library.Service.Interfaces;
using System.Linq.Expressions;

namespace Library.Service.Services;

public class OriginalBookService : BaseService<OriginalBook>, IOriginalBookService
{
    public OriginalBookService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork, validationService)
    {
    }

    public async Task<Result<OriginalBookDto>> GetOriginalBookById(Guid Id)
    {
        var originalBookExistsResult = await _validationService.OriginalBookExists(Id);

        if (originalBookExistsResult.IsFailure)
        {
            return Result.Failure<OriginalBookDto>(originalBookExistsResult.Error);
        }

        return originalBookExistsResult.Value().MapToOriginalBookDto();
    }

    public IEnumerable<OriginalBookDto> GetAllOriginalBooksSorted(bool includeDeleted)
    {
        var books = _unitOfWork.OriginalBooks
            .GetAllAsQueryable()
            .OrderBy(x => x.Title)
            .AsEnumerable();

        if (!includeDeleted)
        {
            books = _unitOfWork.GetBaseModelRepository<OriginalBook>().FilterOutDeleted(books);
        }

        return books.Select(x => x.MapToOriginalBookDto()).ToList();
    }

    public async Task<EntityFiltersDto<OriginalBookDto>> GetAllFilteredOriginalBooks(EntityFiltersDto<OriginalBookDto> originalBookFilters)
    {
        var originalBooks = _unitOfWork.OriginalBooks.GetAllAsQueryable();

        originalBooks = originalBooks.IncludeDeleted(originalBookFilters.IncludeDeleted);
        originalBooks = originalBooks.ApplySearch(originalBookFilters.SearchString, GetOriginalBookSearchProperties());
        originalBookFilters.TotalItems = originalBooks.Count();
        originalBooks = originalBooks.ApplySort(originalBookFilters.SortBy, originalBookFilters.SortOrder, GetOriginalBookSortDictionary());
        var finalBooks = originalBooks.ApplyPagination(originalBookFilters.PageNumber, originalBookFilters.PageSize).ToList();

        var booksDto = finalBooks.Select(b => b.MapToOriginalBookDto()).ToList();

        foreach (var bookDto in booksDto)
        {
            await MapBooks(bookDto);
        }

        originalBookFilters.Entities = booksDto;
        return originalBookFilters;
    }

    public async Task<Result> Create(CreateOriginalBookDto createOriginalBookDto)
    {
        var bookIsNewResult = await _validationService.OriginalBookIsNew(createOriginalBookDto.Title, createOriginalBookDto.OriginalPublishYear, false);

        if (bookIsNewResult.IsFailure)
        {
            return bookIsNewResult.Error;
        }


        var originalBook = createOriginalBookDto.MapToOriginalBook();

        AddGenresToBook(originalBook, createOriginalBookDto.SelectedGenreIds.ToArray());
        await _unitOfWork.OriginalBooks.Create(originalBook);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> Update(OriginalBookDto originalBookDto)
    {
        var originalBookExistsResult = await _validationService.OriginalBookExists(originalBookDto.Id, false);

        if (originalBookExistsResult.IsFailure)
        {
            return Result.Failure(originalBookExistsResult.Error);
        }

        var originalBook = originalBookDto.MapToOriginalBook();
        await _unitOfWork.OriginalBooks.UpdateGenresForBook(originalBookDto.Id, originalBookDto.GenreIds);
        originalBook.UpdateDate = DateTime.UtcNow;

        _unitOfWork.OriginalBooks.Update(originalBook);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    // helpers
    private void AddGenresToBook(OriginalBook book, int[] genreIds)
    {
        foreach (var genreId in genreIds)
        {
            book.BookGenres.Add(new OriginalBookGenre { OriginalBookId = book.Id, GenreId = genreId });
        }
    }

    private async Task MapBooks(OriginalBookDto bookDto)
    {
        var books = await _unitOfWork.Books.GetAllBookEditionsOfOriginalBook(bookDto.Id);
        bookDto.Books = books.Select(b => new BookIdAndTitleDto(b.Id, b.OriginalBook.Title)).ToArray();
    }

    // Returns a dictionary that we will later use in generic sort method
    private static Dictionary<string, Expression<Func<OriginalBook, object>>> GetOriginalBookSortDictionary()
    {
        var dict = new Dictionary<string, Expression<Func<OriginalBook, object>>>
        {
            ["Title"] = b => b.Title,
            ["PublishYear"] = b => b.OriginalPublishYear.ToString()
        };

        return dict;
    }

    // Returns a function that we will use to search items
    private static Func<OriginalBook, string>[] GetOriginalBookSearchProperties()
    {
        return
        [
            b => b.Id.ToString(),
            b => b.Title,
        ];
    }
}
