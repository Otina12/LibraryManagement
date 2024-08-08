using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Report;
using Library.Service.Dtos.Report;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class GenreService : IGenreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public GenreService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }

    public async Task<IEnumerable<Genre>> GetAllGenres()
    {
        return await _unitOfWork.Genres.GetAll();
    }
}
