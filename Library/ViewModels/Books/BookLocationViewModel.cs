namespace Library.ViewModels.Books;

public class BookLocationViewModel
{
    public int RoomId { get; set; }
    public int? ShelfId { get; set; } = 0;
    public int Quantity { get; set; }
}
