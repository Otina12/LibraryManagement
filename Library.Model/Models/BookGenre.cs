using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class BookGenre
{
    [ForeignKey("Book")]
    public Guid BookId { get; set; }

    [ForeignKey("Genre")]
    public int GenreId { get; set; }

    public Book Book { get; set; }
    public Genre Genre { get; set; }
}
