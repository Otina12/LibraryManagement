using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class CustomerRepository : BaseModelRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetById(string Id, bool trackChanges)
    {
        return trackChanges ?
            await dbSet.FirstOrDefaultAsync(c => c.Id == Id) :
            await dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
    }

    public override IQueryable<Customer> GetAllAsQueryable(bool trackChanges)
    {
        return trackChanges ?
            dbSet.Include(c => c.Reservations).AsQueryable() :
            dbSet.Include(c => c.Reservations).AsNoTracking().AsQueryable();
    }
}
