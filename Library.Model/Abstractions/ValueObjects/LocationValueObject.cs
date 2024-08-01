using Library.Model.Enums;

namespace Library.Model.Abstractions.ValueObjects;

public record LocationValueObject(int RoomId, int? ShelfId, Status Status, int Quantity);
