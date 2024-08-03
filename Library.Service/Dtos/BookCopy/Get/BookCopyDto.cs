using Library.Model.Enums;

namespace Library.Service.Dtos.BookCopy.Get;

public record BookCopyDto(
    Guid Id,
    BookCopyStatus Status,
    int RoomId,
    int? ShelfId
    );
