using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Data.Repositories;

public class BaseModelRepository<T> : GenericRepository<T>, IBaseModelRepository<T> where T : BaseModel
{
    public BaseModelRepository(ApplicationDbContext context) : base(context)
    {

    }

    public virtual void Deactivate(T entity)
    {
        entity.DeleteDate = DateTime.UtcNow;
        dbSet.Update(entity);
    }

    public virtual void Reactivate(T entity)
    {
        entity.DeleteDate = null;
        dbSet.Update(entity);
    }

    public virtual IEnumerable<T> FilterOutDeleted(IEnumerable<T> entities)
    {
        return entities.Where(x => x.DeleteDate == null);
    }
}
