using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class Shelf
{
    public int Id { get; set; }

    [ForeignKey("Room")]
    public int RoomId { get; set; }
    public Room Room { get; set; }

    public ICollection<Genre> Genres { get; } = [];
    public ICollection<BookCopy> BookCopies { get; } = [];
}
