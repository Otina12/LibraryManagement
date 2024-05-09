using System.Linq.Expressions;

namespace Library.Model.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteAllWhere(Expression<Func<T, bool>> where);
    Task<T?> GetById(Guid id);
    Task<T?> GetOneWhere(Expression<Func<T, bool>> where);
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> where);
}
