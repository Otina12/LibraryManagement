namespace Library.Service.Dtos.Book.Get;

public record BookLocationDto(
    int RoomId,
    int? ShelfId,
    int Quantity
    );
