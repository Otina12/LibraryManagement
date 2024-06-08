﻿using Library.Model.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class BookCopy : BaseModel
{
    public Guid Id { get; set; }
    public Status Status { get; set; }
    public int Row { get; set; }

    // foreign keys
    public Guid BookId { get; set; }
    public int RoomId { get; set; }
    public int ShelfId { get; set; }


    // navigation properties
    [ForeignKey("BookId")]
    public Book Book { get; set; }

    [ForeignKey("ShelfId, RoomId")]
    public Shelf Shelf { get; set; }

    public ICollection<Reservation> Reservations { get; } = [];

}
