using Library.Model.Interfaces;
using Library.Model.Models;

namespace Library.Service.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<Room>> GetAllRooms()
    {
        return await _unitOfWork.Rooms.GetAll();
    }

    public async Task<IEnumerable<int>> GetAllRoomIds()
    {
        return (await _unitOfWork.Rooms.GetAll())
            .Select(x => x.Id);
    }
}
