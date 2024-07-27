using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class GenreRepository :  GenericRepository<Genre>, IGenreRepository
{
    public GenreRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Genre>> GetAllGenresOfABook(Guid originalBookId)
    {
        var genres = await _context.OriginalBookGenre
            .Include(x => x.Genre)
            .Where(bg => bg.OriginalBookId == originalBookId)
            .ToListAsync();
        return genres.Select(bg => bg.Genre);
    }

    public async Task<IEnumerable<int>> GetAllGenreIdsOfABook(Guid originalBookId)
    {
        var genres = await _context.OriginalBookGenre
            .Where(bg => bg.OriginalBookId == originalBookId)
            .ToListAsync();
        return genres.Select(bg => bg.GenreId);
    }
}
