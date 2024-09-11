using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IOriginalBookRepository : IBaseModelRepository<OriginalBook>
{
    Task<OriginalBookTranslation?> GetTranslationById(Guid bookId, int languageId, bool trackChanges = false);
    Task UpdateGenresForBook(Guid bookId, List<int> newGenreIds);
    void AddOriginalBookTranslation(OriginalBookTranslation translation);
    void UpdateOriginalBookTranslation(OriginalBookTranslation translation);
}
