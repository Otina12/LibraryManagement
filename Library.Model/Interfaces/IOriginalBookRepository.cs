using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IOriginalBookRepository : IBaseModelRepository<OriginalBook>
{
    Task UpdateGenresForBook(Guid bookId, List<int> newGenreIds);
}
