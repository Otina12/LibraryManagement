using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Customers.Post;

namespace Library.Service.Interfaces;

public interface ICustomerService : IBaseService<Customer>
{
    Task<CustomerDto> GetById(string Id);
    Task<EntityFiltersDto<CustomerDto>> GetAllFilteredCustomers(EntityFiltersDto<CustomerDto> customerFilters);
    Task<Result> Create(CreateCustomerDto createCustomerDto);
}
