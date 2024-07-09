using Library.Model.Models;
using Library.Service.Dtos.Reservations.Get;

namespace Library.Service.Helpers.Mappers;

public static class ReservationMapper
{
    public static ReservationDto MapToReservationDto(this Reservation reservation)
    {
        return new ReservationDto(
            reservation.BookCopyId,
            reservation.BookCopy.Book.Title,
            reservation.CustomerId,
            reservation.ReservationDate,
            reservation.SupposedReturnDate,
            reservation.ActualReturnDate,
            reservation.ReturnCustomerId,
            reservation.EmployeeId
            );
    }
}
