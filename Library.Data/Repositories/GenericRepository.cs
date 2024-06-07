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

    public virtual async Task<IEnumerable<T>> GetAll(bool trackChanges = false)
    {
        return trackChanges ?
            await dbSet.ToListAsync() :
            await dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> where, bool trackChanges = false)
    {
        return trackChanges ?
            await dbSet.Where(where).ToListAsync() :
            await dbSet.AsNoTracking().Where(where).ToListAsync();

    }

    public virtual async Task<T?> GetById(Guid id, bool trackChanges = false) // we cannot implement trackChanges here becase of FindAsync, but will override this when needed
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<T?> GetOneWhere(Expression<Func<T, bool>> where, bool trackChanges = false)
    {
        return trackChanges ?
            await dbSet.FirstOrDefaultAsync(where) :
            await dbSet.AsNoTracking().FirstOrDefaultAsync(where);
    }
}
