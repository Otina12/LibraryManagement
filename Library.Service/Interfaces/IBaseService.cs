using Library.Model.Abstractions;
using Library.Model.Models;

namespace Library.Service.Interfaces;

public interface IBaseService<T> where T : BaseModel
{
    Task<Result<T>> Reactivate(Guid id);
    Task<Result<T>> Deactivate(Guid id);
}
