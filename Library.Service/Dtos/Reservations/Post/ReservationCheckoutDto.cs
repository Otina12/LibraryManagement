using Library.Service.Dtos.ReservationCopy.Post;

namespace Library.Service.Dtos.Reservations.Post;

public record ReservationCheckoutDto(
    Guid ReservationId
    )
{
    public List<ReservationCopyCheckoutDto> ReservationCopyCheckouts { get; set; } = [];
}
