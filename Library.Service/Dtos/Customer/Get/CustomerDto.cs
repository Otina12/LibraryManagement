using Library.Model.Models;

namespace Library.Service.Dtos.Customers.Get;

public record CustomerDto(
    string Id,
    string Name,
    string Surname,
    string Email,
    string PhoneNumber,
    string Address,
    DateTime MembershipStartDate
    )
{
    public IEnumerable<Reservation> Reservations { get; set; } = [];
}
