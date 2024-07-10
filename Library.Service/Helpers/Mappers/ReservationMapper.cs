using Library.Model.Models;
using Library.Service.Dtos.Reservations.Get;

namespace Library.Service.Helpers.Mappers;

public static class ReservationMapper
{
    //public static ReservationDto MapToReservationDto(this Reservation reservation) // only use when Reservation includes bookcopy and book
    //{
    //    return new ReservationDto(
    //        reservation.BookCopyId,
    //        reservation.BookCopy.BookId,
    //        reservation.BookCopy.Book.Title,
    //        reservation.CustomerId,
    //        reservation.ReservationDate,
    //        reservation.SupposedReturnDate,
    //        reservation.ActualReturnDate,
    //        reservation.ReturnCustomerId,
    //        reservation.EmployeeId
    //        );
    //}
}
