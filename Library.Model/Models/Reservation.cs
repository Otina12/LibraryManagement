﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class Reservation : BaseModel
{
    public Guid Id { get; set; }
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }

    [ForeignKey(nameof(Customer))]
    public required string CustomerId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int Quantity { get; set; }
    public DateTime SupposedReturnDate { get; set; }

    [ForeignKey(nameof(Employee))]
    public string EmployeeId { get; set; } // tells us which employee handled reservation

    public Book Book { get; set; }
    public Customer Customer { get; set; }
    public Employee Employee { get; set; }
    public IEnumerable<ReservationCopy> ReservationCopies { get; set; } = [];

}
