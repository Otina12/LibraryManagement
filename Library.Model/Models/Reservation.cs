using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class Reservation : BaseModel
{
    [ForeignKey("BookCopy")]
    public Guid BookCopyId { get; set; }

    [ForeignKey("Customer")]
    public required string CustomerId { get; set; }

    public DateTime ReservationDate { get; set; }
    public DateTime SupposedReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public string? ReturnCustomerId { get; set; } // not FK

    [ForeignKey("Employee")]
    public string EmployeeId { get; set; } // which employee handled reservation

    public BookCopy BookCopy { get; set; }
    public Customer Customer { get; set; }
    public Employee Employee { get; set; }

}
