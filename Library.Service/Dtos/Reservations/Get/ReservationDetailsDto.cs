using Library.Model.Models;
using Library.Service.Dtos.ReservationCopy.Get;

namespace Library.Service.Dtos.Reservations.Get;

public record ReservationDetailsDto(
    Customer Customer,
    Guid ReservationId,
    DateTime SupposedReturnDate,
    Model.Models.Book Book,
    int QuantityToReturn // if it's zero, reservation is complete
    )
{
    public List<ReservationCopyDto> ReservationCopies { get; set; } = [];
    public List<ReservationDetailsDto> OtherReservationsOfCustomer { get; set; } = [];
}
