namespace Library.Service.Dtos.Reservations.Post;

public class CreateReservationDto
{
    public string CustomerId { get; set; }
    public List<BooksReservationDto> Books { get; set; } = [];
}

public class BooksReservationDto {
    public Guid OriginalBookId { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public DateTime SupposedReturnDate { get; set; }
}
