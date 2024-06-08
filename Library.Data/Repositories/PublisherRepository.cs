using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories
{
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<Publisher?> GetById(Guid id, bool trackChanges = false)
        {
            return trackChanges ?
                await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id) :
                await _context.Publishers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Publisher?> GetByEmail(string email)
        {
            var publisher = await _context.Publishers.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            return publisher;
        }

        public async Task<Publisher?> GetByName(string name)
        {
            var publisher = await _context.Publishers.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
            return publisher;
        }

        public async Task<Publisher?> PublisherExists(string email, string name)
        {
            var publisher = await _context.Publishers.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email || x.Name == name);
            return publisher;
        }
    }
}
