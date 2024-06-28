using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class GenreRepository :  GenericRepository<Genre>, IGenreRepository
{
    public GenreRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Genre>> GetAllGenresOfABook(Guid bookId)
    {
        var genres = await _context.BookGenre.Include(x => x.Genre).Where(bg => bg.BookId == bookId).ToListAsync();
        return genres.Select(bg => bg.Genre);
    }

    public async Task<IEnumerable<int>> GetAllGenreIdsOfABook(Guid bookId)
    {
        var genres = await _context.BookGenre.Where(bg => bg.BookId == bookId).ToListAsync();
        return genres.Select(bg => bg.GenreId);
    }
}
