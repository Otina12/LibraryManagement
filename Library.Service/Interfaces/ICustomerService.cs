using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Customers.Post;

namespace Library.Service.Interfaces;

public interface ICustomerService : IBaseService<Customer>
{
    Task<CustomerDto?> GetById(string Id);
    EntityFiltersDto<CustomerDto> GetAllFilteredCustomers(EntityFiltersDto<CustomerDto> customerFilters);
    Task<Result> Create(CreateCustomerDto createCustomerDto);
}
