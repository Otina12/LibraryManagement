using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class ShelfService : IShelfService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShelfService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Shelf>> GetAllShelvesOfRoom(int roomId)
    {
        return await _unitOfWork.Shelves.GetAllWhere(x => x.RoomId == roomId);
    }

    public async Task<IEnumerable<int>> GetAllShelvesIdsOfRoom(int roomId)
    {
        var shelves = await _unitOfWork.Shelves.GetAllWhere(x => x.RoomId == roomId);
        return shelves.Select(x => x.Id);
    }


    public async Task<Dictionary<int, IEnumerable<int>>> GetRoomShelves()
    {
        var roomsAndShelves = new Dictionary<int, IEnumerable<int>>();

        var keys = (await _unitOfWork.Rooms.GetAll()).Select(x => x.Id);

        foreach(var key in keys)
        {
            var shelvesOfRoom = await GetAllShelvesIdsOfRoom(key);
            roomsAndShelves.Add(key, shelvesOfRoom);
        }

        return roomsAndShelves;
    }
}
