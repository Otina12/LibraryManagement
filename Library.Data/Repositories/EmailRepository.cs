using Library.Model.Interfaces;
using Library.Model.Models.Email;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class EmailRepository : GenericRepository<EmailModel>, IEmailRepository
{
    public EmailRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<EmailModel?> GetBySubject(string subject, bool trackChanges)
    {
        return trackChanges ?
            await _context.EmailModels.FirstOrDefaultAsync(x => x.Subject == subject) :
            await _context.EmailModels.AsNoTracking().FirstOrDefaultAsync(x => x.Subject == subject);
    }


}
