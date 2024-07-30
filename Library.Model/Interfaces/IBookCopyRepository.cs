using Library.Model.Abstractions.ValueObjects;
using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IBookCopyRepository : IBaseModelRepository<BookCopy>
{
    Task<IEnumerable<LocationValueObject>> GetAllLocationsOfABook(Guid bookId);
    Task<IEnumerable<BookCopy>> GetXBookCopies(Guid bookId, int X, bool trackChanges = false);
    void AddXBookCopies(Guid bookId, int roomId, int? shelfId, int X, string creationComment);
}
