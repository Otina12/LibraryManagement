using Library.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Customers;

public class CreateCustomerViewModel
{
    [Length(9, 11)]
    [RegularExpression("^\\d+$", ErrorMessage = "Incorrect ID format")]
    public string Id { get; set; } // passport ID
    public string Name { get; set; }
    public string Surname { get; set; }
    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string Email { get; set; }
    [Phone(ErrorMessage = "Wrong phone number format")]
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public ICollection<Reservation> Reservations { get; } = [];
}
