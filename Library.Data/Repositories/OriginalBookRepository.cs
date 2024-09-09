using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class OriginalBookRepository : BaseModelRepository<OriginalBook>, IOriginalBookRepository
{
    public OriginalBookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<OriginalBook?> GetById(Guid Id, bool trackChanges, int languageId)
    {
        var query = _context.OriginalBooks
            .Join(_context.OriginalBookTranslations,
                  ob => ob.Id,
                  obt => obt.OriginalBookId,
                  (ob, obt) => new { OriginalBook = ob, Translation = obt })
            .Where(x => x.OriginalBook.Id == Id && x.Translation.LanguageId == languageId)
            .Select(x => new OriginalBook()
            {
                Id = x.OriginalBook.Id,
                OriginalPublishYear = x.OriginalBook.OriginalPublishYear,
                Title = x.Translation.Title,
                Description = x.Translation.Description ?? ""
            });

        return trackChanges ?
            await query.FirstOrDefaultAsync() :
            await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public override async Task<IEnumerable<OriginalBook>> GetAll(bool trackChanges, int languageId)
    {
        var query = _context.OriginalBooks
            .Join(_context.OriginalBookTranslations,
                  ob => ob.Id,
                  obt => obt.OriginalBookId,
                  (ob, obt) => new { OriginalBook = ob, Translation = obt })
            .Where(x => x.Translation.LanguageId == languageId)
            .Select(x => new OriginalBook()
            {
                Id = x.OriginalBook.Id,
                OriginalPublishYear = x.OriginalBook.OriginalPublishYear,
                Title = x.Translation.Title,
                Description = x.Translation.Description ?? ""
            });

        return trackChanges ?
            await query.ToListAsync() :
            await query.AsNoTracking().ToListAsync();
    }

    public override IQueryable<OriginalBook> GetAllAsQueryable(bool trackChanges, int languageId)
    {
        var query = _context.OriginalBooks.AsQueryable()
            .Join(_context.OriginalBookTranslations,
                  ob => ob.Id,
                  obt => obt.OriginalBookId,
                  (ob, obt) => new { OriginalBook = ob, Translation = obt })
            .Where(x => x.Translation.LanguageId == languageId)
            .Select(x => new OriginalBook()
            {
                Id = x.OriginalBook.Id,
                OriginalPublishYear = x.OriginalBook.OriginalPublishYear,
                Title = x.Translation.Title,
                Description = x.Translation.Description ?? ""
            });

        return trackChanges ?
            query :
            query.AsNoTracking();
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
}
