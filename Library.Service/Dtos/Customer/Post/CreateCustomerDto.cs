using Library.Model.Models;

namespace Library.Service.Dtos.Customers.Post;

public record CreateCustomerDto(
    string Id,
    string Name,
    string Surname,
    string Email,
    string PhoneNumber,
    string Address
    );
