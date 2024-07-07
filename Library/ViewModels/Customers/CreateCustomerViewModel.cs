using Library.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Customers;

public class CreateCustomerViewModel
{
    public string Id { get; set; } // passport ID
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public ICollection<Reservation> Reservations { get; } = [];
}
