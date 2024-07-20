using Library.Service.Dtos.BookCopy.Get;

namespace Library.Service.Dtos.ReservationCopy.Get;

public record ReservationCopyDto(
    Guid ReservationCopyId,
    Guid ReservationId,
    BookCopyDto BookCopyDto
    );
