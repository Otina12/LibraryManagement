using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.OriginalBook.Get;
using Library.Service.Dtos.OriginalBook.Post;
using Library.Service.Helpers;
using Library.Service.Helpers.Mappers;
using Library.Service.Interfaces;
using System.Linq.Expressions;

namespace Library.Service.Services;

public class OriginalBookService : BaseService<OriginalBook>, IOriginalBookService
{
    public OriginalBookService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Result<OriginalBookDto>> GetOriginalBookById(Guid Id, string culture = "en")
    {
        var originalBook = await _unitOfWork.OriginalBooks.GetById(Id);

        if (originalBook is null)
        {
            return Result.Failure<OriginalBookDto>(Error<OriginalBook>.NotFound);
        }

        var translatedOriginalBook = await _unitOfWork.Translations.TranslateOriginalBook(originalBook, LocalizationMapper.LanguageToID(culture));

        return translatedOriginalBook.MapToOriginalBookDto();
    }

    public async Task<Result<EditOriginalBookDto>> GetOriginalBookForEditById(Guid Id)
    {
        var originalBook = await _unitOfWork.OriginalBooks.GetById(Id);

        if (originalBook is null)
        {
            return Result.Failure<EditOriginalBookDto>(Error<OriginalBook>.NotFound);
        }

        var editOriginalBookDto = new EditOriginalBookDto(originalBook.Id, originalBook.OriginalPublishYear, originalBook.CreationDate, originalBook.IsDeleted);

        var languages = await _unitOfWork.Languages.GetAll();
        var translations = new Dictionary<int, (string Title, string Description)>();

        foreach (var language in languages)
        {
            var translation = await _unitOfWork.Translations.TranslateOriginalBook(originalBook, language.Id);
            translations[language.Id] = (translation.Title, translation.Description);
        }

        // hard coded (don't know how to rewrite yet):
        editOriginalBookDto.EnglishTitle = translations[1].Title;
        editOriginalBookDto.EnglishDescription = translations[1].Description;

        editOriginalBookDto.GermanTitle = translations[2].Title;
        editOriginalBookDto.GermanDescription = translations[2].Description;

        editOriginalBookDto.GeorgianTitle = translations[3].Title;
        editOriginalBookDto.GeorgianDescription = translations[3].Description;

        return Result.Success(editOriginalBookDto);
    }

    public async Task<IEnumerable<OriginalBookDto>> GetAllOriginalBooksSorted(bool includeDeleted, string culture = "en")
    {
        var books = _unitOfWork.OriginalBooks
            .GetAllAsQueryable()
            .OrderBy(x => x.Title)
            .AsEnumerable();

        if (!includeDeleted)
        {
            books = _unitOfWork.GetBaseModelRepository<OriginalBook>().FilterOutDeleted(books);
        }

        var translatedBooks = new List<OriginalBook>();

        foreach(var book in books)
        {
            translatedBooks.Add(await _unitOfWork.Translations.TranslateOriginalBook(book, LocalizationMapper.LanguageToID(culture)));
        }

        return books.Select(x => x.MapToOriginalBookDto()).ToList();
    }

    public async Task<EntityFiltersDto<OriginalBookDto>> GetAllFilteredOriginalBooks(EntityFiltersDto<OriginalBookDto> originalBookFilters, string culture = "en")
    {
        var originalBooksTemp = await _unitOfWork.OriginalBooks.GetAll();
        var originalBooks = originalBooksTemp.AsQueryable();

        if (culture != "en")
        {
            var translatedBooks = new List<OriginalBook>();

            foreach (var book in originalBooksTemp)
            {
                translatedBooks.Add(await _unitOfWork.Translations.TranslateOriginalBook(book, LocalizationMapper.LanguageToID(culture)));
            }

            originalBooks = translatedBooks.AsQueryable();
        }

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
        var originalBookFromDb = await _unitOfWork.OriginalBooks.GetOneWhere(x => x.Title == createOriginalBookDto.EnglishTitle && x.OriginalPublishYear == createOriginalBookDto.OriginalPublishYear);

        if (originalBookFromDb is not null)
        {
            return Result.Failure(Error<OriginalBook>.AlreadyExists);
        }

        var originalBook = createOriginalBookDto.MapToOriginalBook();
        AddGenresToBook(originalBook, createOriginalBookDto.SelectedGenreIds.ToArray());

        await _unitOfWork.OriginalBooks.Create(originalBook); // first add book
        await CreateOriginalBookTranslations(originalBook.Id, createOriginalBookDto); // then add all translations
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> Update(EditOriginalBookDto editOriginalBookDto)
    {
        var originalBookFromDb = await _unitOfWork.OriginalBooks.GetById(editOriginalBookDto.Id);

        if (originalBookFromDb is null)
        {
            return Result.Failure(Error<OriginalBook>.NotFound);
        }

        var originalBook = editOriginalBookDto.MapToOriginalBook();
        await _unitOfWork.OriginalBooks.UpdateGenresForBook(editOriginalBookDto.Id, editOriginalBookDto.GenreIds);
        originalBook.UpdateDate = DateTime.UtcNow;

        _unitOfWork.OriginalBooks.Update(originalBook); // update book
        await UpdateOriginalBookTranslations(originalBook.Id, editOriginalBookDto); // update its translations
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    // helpers
    private async Task CreateOriginalBookTranslations(Guid bookId, CreateOriginalBookDto bookDto)
    {
        // even if title and description are empty, we still add them as string.Empty
        var languages = await _unitOfWork.Languages.GetAll();
        for (int i = 1; i <= languages.Count(); i++)
        {
            var (title, description) = bookDto.GetTranslation(i);
            var translationDto = new CreateOriginalBookTranslationDto(bookId, i, title, description);
            _unitOfWork.OriginalBooks.AddOriginalBookTranslation(translationDto.MapToTranslation());
        }
    }

    private async Task UpdateOriginalBookTranslations(Guid bookId, EditOriginalBookDto bookDto)
    {
        // even if title and description are empty, we still add them as string.Empty
        var languages = await _unitOfWork.Languages.GetAll();
        for (int i = 1; i <= languages.Count(); i++)
        {
            var translation = await _unitOfWork.OriginalBooks.GetTranslationById(bookId, i, true);
            var (title, desc) = bookDto.GetTranslation(i);
            if (translation is null) // this will only be null when book was created and then later some language was added for which translation was not created.
            {
                var translationDto = new CreateOriginalBookTranslationDto(bookId, i, title, desc);
                _unitOfWork.OriginalBooks.AddOriginalBookTranslation(translationDto.MapToTranslation());
            }
            else
            {
                translation.Title = title;
                translation.Description = desc;
                _unitOfWork.OriginalBooks.UpdateOriginalBookTranslation(translation);
            }
        }
    }

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
