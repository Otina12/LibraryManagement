using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class ShelfGenre
{
    public int ShelfId { get; set; }

    public int RoomId { get; set; }
    public int GenreId { get; set; }


    [ForeignKey("ShelfId, RoomId")]
    public Shelf Shelf { get; set; }
    public Genre Genre { get; set; }
}
