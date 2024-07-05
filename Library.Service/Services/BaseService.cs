using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Interfaces;
using Library.Service.Services.Logger;

namespace Library.Service.Services;

public class BaseService<T> : IBaseService<T> where T : BaseModel
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IValidationService _validationService;
    private readonly ILoggerManager? _logger;
    private readonly IBaseModelRepository<T> _repository;

    public BaseService(IUnitOfWork unitOfWork, IValidationService validationService, ILoggerManager logger)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _logger = logger;
        _repository = _unitOfWork.GetBaseModelRepository<T>();
    }

    protected BaseService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
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
        _logger?.LogInfo($"Deactivated {entity.GetType()} with Id: {id}");
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
        _logger?.LogInfo($"Reactivated {entity.GetType()} with Id: {id}");
        return Result.Success(entity);
    }
}
