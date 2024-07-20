using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IReservationCopyRepository : IGenericRepository<ReservationCopy>
{
    Task<IEnumerable<BookCopy>> GetAllReservedBookCopiesOfReservation(Guid reservationId);
    Task<IEnumerable<ReservationCopy>> GetAllReservationCopiesOfReservation(Guid reservationId);
    Task<Dictionary<Reservation, IEnumerable<ReservationCopy>>> GetAllReservationCopiesOfReservations(IEnumerable<Reservation> reservations);
    Task<Dictionary<Reservation, IEnumerable<ReservationCopy>>> GetUpcomingReservationCopiesOfReservations(IEnumerable<Reservation> reservations);

}
