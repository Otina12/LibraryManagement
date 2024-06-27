using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class BaseService<T> : IBaseService<T> where T : BaseModel
{
    protected readonly IUnitOfWork _unitOfWork;
    private readonly IBaseModelRepository<T> _repository;

    public BaseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.GetBaseModelRepository<T>();
    }

    public async Task<Result<T>> Deactivate(Guid id)
    {
        var entity = await _repository.GetById(id);
        if (entity is null)
        {
            return Result.Failure(Error<T>.NotFound);
        }

        _repository.Deactivate(entity);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(entity);
    }

    public async Task<Result<T>> Reactivate(Guid id)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
        {
            return Result.Failure(Error<T>.NotFound);
        }

        _repository.Reactivate(entity);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(entity);
    }
}
