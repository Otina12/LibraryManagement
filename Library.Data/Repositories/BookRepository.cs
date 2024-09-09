﻿using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;
public class BookRepository : BaseModelRepository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Book?> GetById(Guid id, bool trackChanges, int languageId)
    {
        return trackChanges ?
            await _context.Books.Include(x => x.OriginalBook).FirstOrDefaultAsync(x => x.Id == id) :
            await _context.Books.Include(x => x.OriginalBook).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<IEnumerable<Book>> GetAll(bool trackChanges, int languageId)
    {
        return trackChanges ?
            await _context.Books.Include(x => x.OriginalBook).ToListAsync() :
            await _context.Books.Include(x => x.OriginalBook).AsNoTracking().ToListAsync();
    }

    public override IQueryable<Book> GetAllAsQueryable(bool trackChanges, int languageId)
    {
        return trackChanges ?
            _context.Books.Include(x => x.OriginalBook).AsQueryable() :
            _context.Books.Include(x => x.OriginalBook).AsNoTracking().AsQueryable();
    }

    public async Task<IEnumerable<Book>> GetAllBooksOfAuthor(Guid authorId, bool trackChanges)
    {
        var query = _context.BookAuthor
                        .Include(x => x.Book)
                        .ThenInclude(x => x.OriginalBook)
                        .Where(x => x.AuthorId == authorId)
                        .Select(x => x.Book);

        return trackChanges ?
            await query.ToListAsync() :
            await query.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetAllBooksOfPublisher(Guid publisherId, bool trackChanges)
    {
        var query = _context.Books.Include(x => x.OriginalBook)
                        .Where(x => x.PublisherId == publisherId);

        return trackChanges ?
            await query.ToListAsync() :
            await query.AsNoTracking().ToListAsync();
    }

    public async Task UpdateAuthorsForBook(Guid bookId, List<Guid> newAuthorIds)
    {
        var existingAuthors = await _context.BookAuthor
                                .Where(ba => ba.BookId == bookId)
                                .ToListAsync();

        var authorsToRemove = existingAuthors
                                .Where(ba => !newAuthorIds.Contains(ba.AuthorId))
                                .ToList();

        var authorsToAdd = newAuthorIds
                                .Except(existingAuthors.Select(ba => ba.AuthorId))
                                .Select(authorId => new BookAuthor { BookId = bookId, AuthorId = authorId })
                                .ToList();

        _context.BookAuthor.RemoveRange(authorsToRemove);
        await _context.BookAuthor.AddRangeAsync(authorsToAdd);
    }

    public async Task<IEnumerable<Book>> GetAllBookEditionsOfOriginalBook(Guid originalBookId, bool trackChanges)
    {
        return trackChanges ?
            await _context.Books
                .Include(x => x.Publisher)
                .Include(x => x.OriginalBook)
                .Where(x => x.OriginalBookId == originalBookId)
                .ToListAsync() :
            await _context.Books
                .Include(x => x.Publisher)
                .Include(x => x.OriginalBook)
                .AsNoTracking()
                .Where(x => x.OriginalBookId == originalBookId)
                .ToListAsync();
    }
}
