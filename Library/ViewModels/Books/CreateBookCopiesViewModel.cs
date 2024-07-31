using Library.Service.Dtos.Book.Get;

namespace Library.ViewModels.Books;

public class CreateBookCopiesViewModel
{
    public Guid BookId { get; set; }
    public List<BookLocationDto> Locations { get; set; } = [];
}
