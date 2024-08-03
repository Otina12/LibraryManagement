using Library.Model.Enums;

namespace Library.Model.Abstractions.ValueObjects;

public record LocationValueObject(int RoomId, int? ShelfId, BookCopyStatus Status, int Quantity);
