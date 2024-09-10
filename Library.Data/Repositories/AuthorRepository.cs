using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class AuthorRepository : BaseModelRepository<Author>, IAuthorRepository
{
    public AuthorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Author?> GetById(Guid id, bool trackChanges)
    {
        return trackChanges ?
            await _context.Authors.FirstOrDefaultAsync(x => x.Id == id) :
            await _context.Authors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Author?> GetByEmail(string email)
    {
        var author = await _context.Authors.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        return author;
    }

    public async Task<Author?> GetByName(string name)
    {
        var author = await _context.Authors.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
        return author;
    }

    public async Task<Author?> AuthorExists(string email, string name)
    {
        var author = await _context.Authors.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email || x.Name == name);
        return author;
    }

    public async Task<IEnumerable<Author>> GetAuthorsOfABook(Guid bookId)
    {
        var authors = await _context.BookAuthor
            .AsNoTracking()
            .Include(ba => ba.Author)
            .Where(ba => ba.BookId == bookId)
            .Select(x => x.Author)
            .ToListAsync();

        return authors;
    }

    public async Task<IEnumerable<Guid>> GetAuthorIdsOfABook(Guid bookId)
    {
        var authors = await GetAuthorsOfABook(bookId);
        return authors.Select(x => x.Id);
    }
}
