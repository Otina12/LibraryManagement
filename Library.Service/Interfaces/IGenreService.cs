using Library.Model.Models;
using Library.Model.Models.Report;
using Library.Service.Dtos.Report;

namespace Library.Service.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<Genre>> GetAllGenres();
}
