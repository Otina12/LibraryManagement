using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories
{
    public class PublisherRepository : BaseModelRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<Publisher?> GetById(Guid id, bool trackChanges)
        {
            return trackChanges ?
                await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id) :
                await _context.Publishers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Publisher?> GetByEmail(string email)
        {
            var publisher = await _context.Publishers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
            return publisher;
        }

        public async Task<Publisher?> GetByName(string name)
        {
            var publisher = await _context.Publishers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name);
            return publisher;
        }

        public async Task<Publisher?> PublisherExists(string email, string name)
        {
            var publisher = await _context.Publishers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email || x.Name == name);
            return publisher;
        }

        public async Task<Publisher?> GetPublisherOfABook(Guid bookId)
        {
            var publisher = (
                await _context.Books
                .Include(b => b.Publisher)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == bookId)
                )?
                .Publisher;

            return publisher;
        }
    }
}
