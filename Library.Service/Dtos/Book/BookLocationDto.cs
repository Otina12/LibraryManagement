namespace Library.Service.Dtos.Book;

public record BookLocationDto(
    int RoomId,
    int? ShelfId,
    int Quantity
    );
