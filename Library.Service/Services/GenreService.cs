using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Report;
using Library.Service.Dtos.Report;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class GenreService : IGenreService
{
    private readonly IUnitOfWork _unitOfWork;

    public GenreService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Genre>> GetAllGenres()
    {
        return await _unitOfWork.Genres.GetAll();
    }
}
