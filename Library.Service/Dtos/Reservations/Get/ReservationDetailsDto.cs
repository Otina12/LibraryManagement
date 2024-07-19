using Library.Model.Enums;
using Library.Model.Models;
using Library.Service.Dtos.BookCopy.Get;

namespace Library.Service.Dtos.Reservations.Get;

public record ReservationDetailsDto(
    Customer Customer,
    Guid ReservationId,
    DateTime SupposedReturnDate,
    Model.Models.Book Book,
    int QuantityToReturn)
{
    public List<BookCopyDto> BookCopies { get; set; } = [];
    public List<ReservationDetailsDto> OtherReservationsOfCustomer { get; set; } = [];
}
