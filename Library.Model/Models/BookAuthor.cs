using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class BookAuthor
{
    [ForeignKey("Book")]
    public Guid BookId { get; set; }

    [ForeignKey("Author")]
    public Guid AuthorId { get; set; }

    public Book Book { get; set; }
    public Author Author { get; set; }
}
