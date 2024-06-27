namespace Library.Data.Repositories;

using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

internal class BookRepository : BaseModelRepository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async override Task<Book?> GetById(Guid id, bool trackChanges)
    {
        return trackChanges ?
            await _context.Books.FirstOrDefaultAsync(x => x.Id == id) :
            await _context.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAllBooksOfAuthor(Guid authorId, bool trackChanges)
    {
        var query = _context.BookAuthor
                        .Include(x => x.Book)
                        .Where(x => x.AuthorId == authorId)
                        .Select(x => x.Book);

        return trackChanges ?
            await query.ToListAsync() :
            await query.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetAllBooksOfPublisher(Guid publisherId, bool trackChanges)
    {
        var query = _context.Books
                        .Where(x => x.PublisherId == publisherId);

        return trackChanges ?
            await query.ToListAsync() :
            await query.AsNoTracking().ToListAsync();
    }

    
}
