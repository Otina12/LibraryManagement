namespace Library.Service.Dtos.Reservations.Get;

public record ReservationDto(
    Guid Id,
    Guid BookId,
    string BookTitle,
    string CustomerId,
    int QuantityToReturn,
    DateTime ReservationDate,
    DateTime SupposedReturnDate,
    string EmployeeId
    );
