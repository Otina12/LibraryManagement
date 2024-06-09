using System.Linq.Expressions;

namespace Library.Model.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteAllWhere(Expression<Func<T, bool>> where);
    Task<T?> GetById(Guid id, bool trackChanges = false);
    Task<T?> GetOneWhere(Expression<Func<T, bool>> where, bool trackChanges = false);
    Task<IEnumerable<T>> GetAll(bool trackChanges = false);
    Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> where, bool trackChanges = false);
}
