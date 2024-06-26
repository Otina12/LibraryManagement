using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<int>> GetAllRoomIds();
    Task<IEnumerable<Room>> GetAllRooms();
}
