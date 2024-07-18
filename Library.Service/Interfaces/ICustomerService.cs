using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Customers.Post;

namespace Library.Service.Interfaces;

public interface ICustomerService : IBaseService<Customer>
{
    Task<Result<CustomerDto>> GetCustomerById(string Id);
    EntityFiltersDto<CustomerDto> GetAllFilteredCustomers(EntityFiltersDto<CustomerDto> customerFilters);
    Task<Result> Create(CreateCustomerDto createCustomerDto);
    Task<Result> Update(CustomerDto customerDto);
    Task<Result<Customer>> Deactivate(string stringId);
    Task<Result<Customer>> Reactivate(string stringId);
}
