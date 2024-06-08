using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<Author?> GetByEmail(string email);
    Task<Author?> GetByName(string name);
    Task<Author?> AuthorExists(string email, string name);
    Task<IEnumerable<Author>> GetAuthorsOfABook(Guid bookId);
}
