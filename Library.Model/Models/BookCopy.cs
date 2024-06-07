using Library.Model.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class BookCopy : BaseModel
{
    public Guid Id { get; set; }
    public int Edition { get; set; }
    public int PageCount { get; set; }
    public Status Status { get; set; }
    public int Row { get; set; }

    [ForeignKey("Publisher")]
    public Guid PublisherId { get; set; }

    [ForeignKey("Book")]
    public Guid BookId { get; set; }

    public int RoomId { get; set; }
    public int ShelfId { get; set; }


    // navigation properties
    public Publisher Publisher { get; set; }
    public Book Book { get; set; }

    [ForeignKey("ShelfId, RoomId")]
    public Shelf Shelf { get; set; }

    public ICollection<Reservation> Reservations { get; } = [];

}
