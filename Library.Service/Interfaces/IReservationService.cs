using Library.Model.Abstractions;
using Library.Service.Dtos.Reservations.Get;
using Library.Service.Dtos.Reservations.Post;

namespace Library.Service.Interfaces;

public interface IReservationService
{
    IEnumerable<(DateTime, IEnumerable<ReservationDto>)> GetAll();
    Task<Result> Create(string employeeId, CreateReservationDto reservationDto);
}
