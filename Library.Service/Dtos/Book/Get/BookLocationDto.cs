using Library.Model.Enums;

namespace Library.Service.Dtos.Book.Get;

public record BookLocationDto(
    int RoomId,
    int? ShelfId,
    Status Status,
    int Quantity
    );
