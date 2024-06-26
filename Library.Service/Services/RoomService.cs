using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public RoomService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
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
