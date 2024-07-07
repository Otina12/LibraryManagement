using Library.Model.Abstractions;
using Library.Service.Dtos.Reservations.Post;

namespace Library.Service.Interfaces;

public interface IReservationService
{
    Task<Result> Create(CreateReservationDto reservationDto);
}
