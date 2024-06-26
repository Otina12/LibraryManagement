using Library.Model.Interfaces;
using Library.Model.Models;

namespace Library.Data.Repositories;

public class ShelfRepository : GenericRepository<Shelf>, IShelfRepository
{
    public ShelfRepository(ApplicationDbContext context) : base(context)
    {
    }
}
