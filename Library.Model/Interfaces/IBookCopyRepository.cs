using Library.Model.Abstractions.ValueObjects;
using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IBookCopyRepository : IBaseModelRepository<BookCopy>
{
    Task<IEnumerable<LocationValueObject>> GetAllLocationsOfABook(Guid bookId);
    void DeleteXBookCopies(Guid bookId, int roomId, int? shelfId, int X);
    void AddXBookCopies(Guid bookId, int roomId, int? shelfId, int X);
}
