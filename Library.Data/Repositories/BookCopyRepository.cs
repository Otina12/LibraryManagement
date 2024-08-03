using Library.Model.Abstractions.ValueObjects;
using Library.Model.Enums;
using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class BookCopyRepository : BaseModelRepository<BookCopy>, IBookCopyRepository
{
    public BookCopyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LocationValueObject>> GetAllLocationsOfABook(Guid bookId)
    {
        return await dbSet
            .Where(x => x.BookId == bookId) // get all copies of a book
            .GroupBy(x => new { x.RoomId, x.ShelfId, x.Status }) // group by locations
            .Select(x => new LocationValueObject(x.Key.RoomId, x.Key.ShelfId, x.Key.Status, x.Count())) // retrieve count for each
            .ToListAsync();
    }

    public async Task<int> GetCountOfAvailableBookCopies(Guid bookId)
    {
        return await dbSet
            .Where(x => x.BookId == bookId && !x.IsTaken && x.Status != BookCopyStatus.Lost && x.DeleteDate == null)
            .CountAsync();
    }

    public async Task<IEnumerable<BookCopy>> GetXBookCopies(Guid bookId, int X, bool trackChanges)
    {
        return trackChanges ?
            await dbSet
                .Where(x => x.BookId == bookId && x.Status != BookCopyStatus.Lost && !x.IsTaken) // take not taken and not lost books
                .OrderBy(x => x.Status) // take best quality (low Enum.Status) book copies
                .Take(X)
                .ToListAsync() :
            await dbSet.AsNoTracking()
                .Where(x => x.BookId == bookId && x.Status != BookCopyStatus.Lost && !x.IsTaken)
                .OrderBy(x => x.Status)
                .Take(X)
                .ToListAsync();
    }

    public void AddXBookCopies(Guid bookId, int roomId, int? shelfId, int X, BookCopyStatus status, string creationComment)
    {
        var bookCopies = Enumerable.Range(0, X)
                            .Select(_ => new BookCopy
                            {
                                Status = status,
                                BookId = bookId,
                                RoomId = roomId,
                                ShelfId = shelfId,
                                IsTaken = false,
                                CreationComment = creationComment,
                                CreationDate = DateTime.UtcNow
                            }).ToList();

        dbSet.AddRange(bookCopies);
    }
}
