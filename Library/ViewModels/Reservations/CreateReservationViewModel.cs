namespace Library.ViewModels.Reservations;

public class CreateReservationViewModel
{
    public string CustomerId { get; set; } = null!;
    public List<BooksReservationViewModel> Books { get; set; } = []; // several copies of different books may be reserved
}

public class BooksReservationViewModel
{
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public DateOnly SupposedReturnDate { get; set; }

}