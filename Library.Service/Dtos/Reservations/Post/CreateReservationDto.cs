namespace Library.Service.Dtos.Reservations.Post;

public record CreateReservationDto(string CustomerId)
{
    public BookCopiesDto[] Books { get; set; } = [];
}

public record BookCopiesDto(Guid BookId, int Quantity, DateOnly SupposedReturnDate);
