using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    public AuthorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Author?> GetById(Guid id, bool trackChanges = false)
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
}
