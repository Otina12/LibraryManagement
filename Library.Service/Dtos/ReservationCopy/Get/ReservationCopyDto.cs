using Library.Model.Enums;
using Library.Service.Dtos.BookCopy.Get;

namespace Library.Service.Dtos.ReservationCopy.Get;

public record ReservationCopyDto(
    Guid ReservationCopyId,
    Guid ReservationId,
    Guid BookCopyId,
    Status TakenStatus,
    Status? ReturnedStatus,
    DateTime? ReturnDate,
    int RoomId,
    int? ShelfId
    );
