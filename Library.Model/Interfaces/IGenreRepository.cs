using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IGenreRepository : IGenericRepository<Genre>
{
    Task<IEnumerable<Genre>> GetAllGenresOfABook(Guid bookId);
    Task<IEnumerable<int>> GetAllGenreIdsOfABook(Guid bookId);
}
