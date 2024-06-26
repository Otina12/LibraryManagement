using Library.Model.Interfaces;
using Library.Model.Models;

namespace Library.Data.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
    }
}
