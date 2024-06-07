using Library.Data.Repositories;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public BookService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }
}
