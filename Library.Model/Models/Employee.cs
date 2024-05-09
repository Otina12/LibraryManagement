using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class Employee : IdentityUser
{
    public required string Name { get; set; }
    public required string Surname { get; set; }

    [Column(TypeName = "date")]
    public DateTime DateOfBirth { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }

    public ICollection<Reservation> Reservations { get; } = [];

    public Employee() : base()
    {
        
    }
}
