using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
{
    public LanguageRepository(ApplicationDbContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Language>> GetAllActive()
    {
        return await _context.Languages.Where(x => x.IsActive).ToListAsync();
    }
}
