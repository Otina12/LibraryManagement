using Library.Model.Models;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Customers.Post;

namespace Library.Service.Helpers.Mappers;

public static class CustomerMapper
{
    public static CustomerDto MapToCustomerDto(this Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Surname,
            customer.Email,
            customer.PhoneNumber,
            customer.Address,
            customer.CreationDate,
            customer.IsDeleted
            )
        {
            Reservations = customer.Reservations
        };
    }

    public static Customer MapToCustomer(this CreateCustomerDto customerDto)
    {
        return new Customer
        {
            Id = customerDto.Id,
            Name = customerDto.Name,
            Surname = customerDto.Surname,
            Email = customerDto.Email,
            PhoneNumber = customerDto.PhoneNumber,
            Address = customerDto.Address,
            CreationDate = DateTime.UtcNow
        };
    }

    public static Customer MapToCustomer(this CustomerDto customerDto)
    {
        return new Customer
        {
            Id = customerDto.Id,
            Name = customerDto.Name,
            Surname = customerDto.Surname,
            Email = customerDto.Email,
            PhoneNumber = customerDto.PhoneNumber,
            Address = customerDto.Address,
            CreationDate = customerDto.MembershipStartDate
        };
    }
}
