using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IShelfService
{
    Task<Dictionary<int, IEnumerable<int>>> GetRoomShelves();
    Task<IEnumerable<Shelf>> GetAllShelvesOfRoom(int roomId);
    Task<IEnumerable<int>> GetAllShelvesIdsOfRoom(int roomId);
}
