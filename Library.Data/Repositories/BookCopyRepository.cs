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
        return await _context.BookCopies
            .Where(x => x.BookId == bookId) // get all copies of a book
            .GroupBy(x => new { x.RoomId, x.ShelfId }) // group by locations
            .Select(x => new LocationValueObject(x.Key.RoomId, x.Key.ShelfId, x.Count())) // retrieve count for each
            .ToListAsync();
    }

    public void AddXBookCopies(Guid bookId, int roomId, int? shelfId, int X)
    {
        var bookCopies = Enumerable.Range(0, X)
                            .Select(_ => new BookCopy
                            {
                                Status = Model.Enums.Status.Normal,
                                BookId = bookId,
                                RoomId = roomId,
                                ShelfId = shelfId,
                                IsTaken = false,
                                CreationDate = DateTime.UtcNow
                            }).ToList();

        _context.BookCopies.AddRange(bookCopies);
    }

    public void DeleteXBookCopies(Guid bookId, int roomId, int? shelfId, int X)
    {
        var copiesToDelete = _context.BookCopies
                                .Where(bc => bc.BookId == bookId &&
                                                bc.RoomId == roomId &&
                                                bc.ShelfId == shelfId)
                                .OrderByDescending(bc => bc.Status) // delete higher Enums.Status books (lost, damaged...)
                                .Take(X)
                                .ToList();

        _context.BookCopies.RemoveRange(copiesToDelete);
    }
}
