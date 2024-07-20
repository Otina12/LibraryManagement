using Library.Model.Enums;

namespace Library.Service.Dtos.ReservationCopy.Post;

public record ReservationCopyCheckoutDto(
    Guid ReservationCopyId,
    Guid BookCopyId,
    Status? NewStatus
    );
