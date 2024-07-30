using Library.Model.Abstractions.ValueObjects;
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
            .GroupBy(x => new { x.RoomId, x.ShelfId }) // group by locations
            .Select(x => new LocationValueObject(x.Key.RoomId, x.Key.ShelfId, x.Count())) // retrieve count for each
            .ToListAsync();
    }

    public async Task<IEnumerable<BookCopy>> GetXBookCopies(Guid bookId, int X, bool trackChanges)
    {
        return trackChanges ?
            await dbSet
                .Where(x => x.BookId == bookId && x.Status != Model.Enums.Status.Lost && !x.IsTaken) // take not taken and not lost books
                .OrderBy(x => x.Status) // take best quality (low Enum.Status) book copies
                .Take(X)
                .ToListAsync() :
            await dbSet.AsNoTracking()
                .Where(x => x.BookId == bookId && !x.IsTaken)
                .OrderBy(x => x.Status)
                .Take(X)
                .ToListAsync();
    }

    public void AddXBookCopies(Guid bookId, int roomId, int? shelfId, int X, string creationComment)
    {
        var bookCopies = Enumerable.Range(0, X)
                            .Select(_ => new BookCopy
                            {
                                Status = Model.Enums.Status.Normal,
                                BookId = bookId,
                                RoomId = roomId,
                                ShelfId = shelfId,
                                IsTaken = false,
                                CreationComment = creationComment,
                                CreationDate = DateTime.UtcNow
                            }).ToList();

        dbSet.AddRange(bookCopies);
    }

    public void DeleteXBookCopies(Guid bookId, int roomId, int? shelfId, int X)
    {
        var copiesToDelete = dbSet
                                .Where(bc => bc.BookId == bookId &&
                                                bc.RoomId == roomId &&
                                                bc.ShelfId == shelfId)
                                .OrderByDescending(bc => bc.Status) // delete higher Enums.Status books (lost, damaged...)
                                .Take(X)
                                .ToList();

        dbSet.RemoveRange(copiesToDelete);
    }
}
