using Library.Model.Interfaces;
using Library.Model.Models;

namespace Library.Data.Repositories;

public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
{
    public LanguageRepository(ApplicationDbContext context) : base(context)
    {
    }
}
