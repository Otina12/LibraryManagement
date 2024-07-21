using Library.Model.Enums;

namespace Library.Service.Dtos.ReservationCopy.Post;

public class ReservationCopyCheckoutDto
{
    public Guid ReservationCopyId { get; set; }
    public Guid BookCopyId { get; set; }
    public Status NewStatus { get; set; }
}