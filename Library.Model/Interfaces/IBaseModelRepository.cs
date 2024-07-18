using Library.Model.Models;

namespace Library.Model.Interfaces;

/// <summary>
/// Generic repository for models that inherit from BaseModel
/// </summary>
/// <typeparam name="T">Type of entity managed by the repository.</typeparam>
public interface IBaseModelRepository<T> : IGenericRepository<T> where T : BaseModel
{
    void Deactivate(T entity);
    void Reactivate(T entity);
    IEnumerable<T> FilterOutDeleted(IEnumerable<T> entities);
}
