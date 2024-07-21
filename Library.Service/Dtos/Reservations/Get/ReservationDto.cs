namespace Library.Service.Dtos.Reservations.Get;

public record ReservationDto(
    Guid Id,
    Guid BookId,
    string BookTitle,
    string CustomerId,
    int TotalQuantity,
    int QuantityToReturn,
    DateTime ReservationDate,
    DateTime SupposedReturnDate,
    DateTime? LastCopyReturnDate,
    string EmployeeId
    );
