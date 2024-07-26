using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class OriginalBookRepository : BaseModelRepository<OriginalBook>, IOriginalBookRepository
{
    public OriginalBookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<OriginalBook?> GetById(Guid Id, bool trackChanges)
    {
        return trackChanges ?
            await _context.OriginalBooks.FirstOrDefaultAsync(book => book.Id == Id) :
            await _context.OriginalBooks.AsNoTracking().FirstOrDefaultAsync(book => book.Id == Id);
    }
}
