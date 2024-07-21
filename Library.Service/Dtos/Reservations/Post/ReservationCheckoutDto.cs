using Library.Service.Dtos.ReservationCopy.Post;

namespace Library.Service.Dtos.Reservations.Post;

public class ReservationCheckoutDto
{
    public Guid ReservationId { get; set; }
    public List<ReservationCopyCheckoutDto> ReservationCopyCheckouts { get; set; } = [];
}
