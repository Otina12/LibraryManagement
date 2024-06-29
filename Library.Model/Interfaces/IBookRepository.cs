using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IBookRepository : IBaseModelRepository<Book>
{
    Task<IEnumerable<Book>> GetAllBooksOfAuthor(Guid authorId, bool trackChanges = false);
    Task<IEnumerable<Book>> GetAllBooksOfPublisher(Guid publisherId, bool trackChanges = false);
    Task UpdateGenresForBook(Guid bookId, List<int> newGenreIds);
    Task UpdateAuthorsForBook(Guid bookId, List<Guid> newAuthorsIds);
}
