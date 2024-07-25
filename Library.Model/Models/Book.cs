using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class Book : BaseModel
{
    public Guid Id { get; set; }
    public string ISBN { get; set; }
    public int Edition { get; set; }
    public int PageCount { get; set; }
    public int Quantity { get; set; } = 1;
    public int PublishYear { get; set; }

    [ForeignKey("Publisher")]
    public Guid? PublisherId { get; set; }
    [ForeignKey("OriginalBook")]
    public Guid? OriginalBookId { get; set; }

    // navigation properties
    public Publisher? Publisher { get; set; }
    public OriginalBook OriginalBook { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; } = [];
    public ICollection<BookGenre> BookGenres { get; } = [];
    public ICollection<BookCopy> BookCopies { get; } = [];
    public ICollection<Reservation> Reservations { get; } = [];
}
