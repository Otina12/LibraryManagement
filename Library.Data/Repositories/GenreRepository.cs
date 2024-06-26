using Library.Model.Interfaces;
using Library.Model.Models;

namespace Library.Data.Repositories;

public class GenreRepository :  GenericRepository<Genre>, IGenreRepository
{
    public GenreRepository(ApplicationDbContext context) : base(context)
    {

    }
}
