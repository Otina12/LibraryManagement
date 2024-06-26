using Library.Model.Models;

namespace Library.Service.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<Genre>> GetAllGenres();
}
