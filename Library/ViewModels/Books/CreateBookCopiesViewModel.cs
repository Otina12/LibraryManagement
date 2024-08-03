using Library.Service.Dtos.Book.Get;

namespace Library.ViewModels.Books;

public class CreateBookCopiesViewModel
{
    public Guid BookId { get; set; }
    public string? CreationComment { get; set; }
    public List<BookLocationDto> Locations { get; set; } = [];
    public Guid? ReservationCopyId { get; set; } // in case when we return another copy, we want to make this book copy of Status 'LostAndReturnedAnotherCopy'
}
