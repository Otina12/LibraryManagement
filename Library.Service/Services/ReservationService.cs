using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Service.Dtos.Reservations.Post;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public ReservationService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }

    public Task<Result> Create(CreateReservationDto reservationDto)
    {
        throw new NotImplementedException();
    }
}
