using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class TranslationRepository : ITranslationRepository
{
    private readonly ApplicationDbContext _context;

    public TranslationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // The method returns original book if the translation doesn't exist or its title empty.
    // In the database we will have all key pairs {OriginalBookId, LanguageId} but some of the titles will be empty, meaning there is no translation.
    // This way we don't need to check if translation is null when, for example, editing the translated title.
    public async Task<OriginalBook> TranslateOriginalBook(OriginalBook originalBook, int languageId)
    {
        var translation = await _context.OriginalBookTranslations.AsNoTracking()
            .FirstOrDefaultAsync(x => x.OriginalBookId == originalBook.Id && x.LanguageId == languageId);

        if (translation is null || string.IsNullOrEmpty(translation.Title))
        {
            return originalBook;
        }

        var originalBookCopy = new OriginalBook()
        {
            Id = originalBook.Id,
            OriginalPublishYear = originalBook.OriginalPublishYear,
            Title = translation.Title,
            Description = translation.Description ?? "",
            Books = originalBook.Books,
            BookGenres = originalBook.BookGenres,
            Translations = originalBook.Translations,
            CreationDate = originalBook.CreationDate,
            UpdateDate = originalBook.UpdateDate,
            DeleteDate = originalBook.DeleteDate
        };

        return originalBookCopy;
    }
}
