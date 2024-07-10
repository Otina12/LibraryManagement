using Library.Model.Abstractions;
using Library.Service.Dtos.Reservations.Get;
using Library.Service.Dtos.Reservations.Post;

namespace Library.Service.Interfaces;

public interface IReservationService
{
    //Task<IEnumerable<(DateTime Date, IEnumerable<(Guid BookId, IEnumerable<ReservationDto> Reservations)>)>> GetAll();
    //Task<Result> Create(string employeeId, CreateReservationDto reservationDto);
}
