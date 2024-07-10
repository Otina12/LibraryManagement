namespace Library.Service.Dtos.Reservations.Get;

public record ReservationDto(
    Guid BookId,
    string BookTitle,
    string CustomerId,
    int Quantity,
    DateTime ReservationDate,
    DateTime SupposedReturnDate,
    string EmployeeId
    );
