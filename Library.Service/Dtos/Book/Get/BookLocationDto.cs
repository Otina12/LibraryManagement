using Library.Model.Enums;

namespace Library.Service.Dtos.Book.Get;

public record BookLocationDto(
    int RoomId,
    int? ShelfId,
    BookCopyStatus Status,
    int Quantity
    );
