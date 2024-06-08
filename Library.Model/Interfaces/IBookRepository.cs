using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<IEnumerable<Book>> GetAllBooksOfAuthor(Guid authorId, bool trackChanges = false);
    Task<IEnumerable<Book>> GetAllBooksOfPublisher(Guid publisherId, bool trackChanges = false);
}
