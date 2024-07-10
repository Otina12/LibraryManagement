namespace Library.Service.Dtos.Reservations.Get;

public record ReservationDto(
    Guid BookCopyId,
    Guid BookId,
    string BookTitle,
    string CustomerId,
    DateTime ReservationDate,
    DateTime SupposedReturnDate,
    DateTime? ActualReturnDate,
    string? ReturnCustomerId,
    string EmployeeId
    );
