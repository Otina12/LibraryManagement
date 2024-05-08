using Microsoft.AspNetCore.Identity;

namespace Library.Model.Models;

public class Employee : IdentityUser
{
    public required string Name { get; set; }
    public required string Surname { get; set; }

    public DateTime CreationDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }

    public ICollection<Reservation> Reservations { get; } = [];

    public Employee() : base()
    {
        
    }
}
