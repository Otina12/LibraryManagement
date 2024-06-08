using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class Shelf // is a weak entity of room. Primary key is { roomId, shelfId }
{
    public int Id { get; set; }

    [ForeignKey("Room")]
    public int RoomId { get; set; }
    public Room Room { get; set; }

    public ICollection<BookCopy> BookCopies { get; } = [];
}
