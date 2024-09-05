namespace Library.Model.Models;

public class Customer : BaseModel
{
    public required string Id { get; set; } // passport ID
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public ICollection<Reservation> Reservations { get; } = [];
    public ICollection<BookCopyLog> BookCopyLogs { get; } = [];
}
