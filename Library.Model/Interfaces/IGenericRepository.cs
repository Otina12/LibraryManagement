using System.Linq.Expressions;

namespace Library.Model.Interfaces;

public interface IGenericRepository<T> where T : class
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteAllWhere(Expression<Func<T, bool>> where);
    T GetById(int id);
    T GetOneWhere(Expression<Func<T, bool>> where);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> where);
}
