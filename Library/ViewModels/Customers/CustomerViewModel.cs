using Library.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Customers;

public class CustomerViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string Email { get; set; }
    [MaxLength(25, ErrorMessage = "Phone number must be at most 25 characters long")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime MembershipStartDate { get; set; }
    public IEnumerable<Reservation> Reservations { get; set; } = [];
    public bool isDeleted { get; set; }
}
