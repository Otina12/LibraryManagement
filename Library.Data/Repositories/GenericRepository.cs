using Library.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected ApplicationDbContext _context;
    protected DbSet<T> dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        dbSet = context.Set<T>();
    }
    

    public virtual async Task Create(T entity)
    {
        await dbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        dbSet.Remove(entity);
    }

    public virtual void DeleteAllWhere(Expression<Func<T, bool>> where)
    {
        var entitiesToRemove = dbSet.Where(where);
        dbSet.RemoveRange(entitiesToRemove);
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> where)
    {
        return await dbSet.Where(where).ToListAsync();
    }

    public virtual async Task<T?> GetById(Guid id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<T?> GetOneWhere(Expression<Func<T, bool>> where)
    {
        return await dbSet.FirstOrDefaultAsync(where);
    }
}
