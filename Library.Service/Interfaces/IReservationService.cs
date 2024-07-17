using Library.Model.Abstractions;
using Library.Service.Dtos;
using Library.Service.Dtos.Reservations.Get;
using Library.Service.Dtos.Reservations.Post;

namespace Library.Service.Interfaces;

public interface IReservationService
{
    Task<EntityFiltersDto<(DateTime, IEnumerable<ReservationDto>)>> GetAll(EntityFiltersDto<(DateTime, IEnumerable<ReservationDto>)> reservationFilters);
    Task<Result> CreateReservations(string employeeId, CreateReservationDto createReservationDto);
}
