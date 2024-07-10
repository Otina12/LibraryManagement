using Library.Model.Abstractions;
using Library.Service.Dtos.Reservations.Get;
using Library.Service.Dtos.Reservations.Post;

namespace Library.Service.Interfaces;

public interface IReservationService
{
    Task<IEnumerable<(DateTime, IEnumerable<ReservationDto>)>> GetAll();
    Task<Result> CreateReservations(string employeeId, CreateReservationDto createReservationDto);
}
