using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.Data.Repositories;

public class OriginalBookRepository : BaseModelRepository<OriginalBook>, IOriginalBookRepository
{
    public OriginalBookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<OriginalBook?> GetById(Guid Id, bool trackChanges)
    {
        return trackChanges ?
            await _context.OriginalBooks.FirstOrDefaultAsync(book => book.Id == Id) :
            await _context.OriginalBooks.AsNoTracking().FirstOrDefaultAsync(book => book.Id == Id);
    }

    public async Task UpdateGenresForBook(Guid bookId, List<int> newGenreIds)
    {
        var existingGenres = await _context.OriginalBookGenre
                                .Where(bg => bg.OriginalBookId == bookId)
                                .ToListAsync();

        var genresToRemove = existingGenres
                                .Where(bg => !newGenreIds.Contains(bg.GenreId))
                                .ToList();

        var genresToAdd = newGenreIds
                                .Except(existingGenres.Select(bg => bg.GenreId))
                                .Select(genreId => new OriginalBookGenre { OriginalBookId = bookId, GenreId = genreId })
                                .ToList();

        _context.OriginalBookGenre.RemoveRange(genresToRemove);
        await _context.OriginalBookGenre.AddRangeAsync(genresToAdd);
    }

    private async Task<OriginalBook?> Translate(OriginalBook? originalBook, int languageId)
    {
        if (originalBook is null)
        {
            return null;
        }

        var translation = await _context.OriginalBookTranslations.FirstOrDefaultAsync(x => x.OriginalBookId == originalBook.Id && x.LanguageId == languageId);

        if (translation is null)
        {
            return originalBook;
        }

        var originalBookCopy = new OriginalBook()
        {
            Id = originalBook.Id,
            OriginalPublishYear = originalBook.OriginalPublishYear,
            Title = translation.Title,
            Description = translation.Description ?? "",
            CreationDate = originalBook.CreationDate,
            UpdateDate = originalBook.UpdateDate,
            DeleteDate = originalBook.DeleteDate
        };

        return originalBookCopy;
    }
}
