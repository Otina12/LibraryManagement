using Library.Model.Models;
using Library.Service.Dtos.Reservations.Get;
using Library.Service.Dtos.Reservations.Post;

namespace Library.Service.Helpers.Mappers;

public static class ReservationMapper
{
    public static Reservation MapToReservation(this BooksReservationDto reservationDto, string employeeId, string customerId)
    {
        var dateTimeNow = DateTime.UtcNow;
        return new Reservation
        {
            BookId = reservationDto.BookId,
            CustomerId = customerId,
            ReservationDate = dateTimeNow,
            Quantity = reservationDto.Quantity,
            SupposedReturnDate = reservationDto.SupposedReturnDate,
            EmployeeId = employeeId,
            CreationDate = dateTimeNow
        };
    }

    public static ReservationDto MapToReservationDto(this Reservation reservation) // only use when Reservation includes (is joined with) book
    {
        var dateTimeNow = DateTime.UtcNow;
        return new ReservationDto(
            reservation.Id,
            reservation.BookId,
            reservation.Book.Title,
            reservation.CustomerId,
            reservation.Quantity,
            reservation.Quantity - reservation.ReturnedQuantity,
            reservation.ReservationDate,
            reservation.SupposedReturnDate,
            reservation.LastCopyReturnDate,
            reservation.EmployeeId
            );
    }
}
