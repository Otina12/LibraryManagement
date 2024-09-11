using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface ILanguageRepository : IGenericRepository<Language>
{
    Task<IEnumerable<Language>> GetAllActive();
}
