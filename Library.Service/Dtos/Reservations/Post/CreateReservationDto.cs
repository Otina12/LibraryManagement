namespace Library.Service.Dtos.Reservations.Post;

public record CreateReservationDto(string CustomerId)
{
    BookCopiesDto[] Books { get; set; } = [];
}

public record BookCopiesDto(Guid BookId, int Quantity, DateOnly SupposedReturnDate);
