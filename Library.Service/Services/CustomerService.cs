using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Book;
using Library.Service.Helpers.Extensions;
using Library.Service.Helpers;
using Library.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Library.Service.Helpers.Mappers;
using Library.Model.Abstractions;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Customers.Post;
using Library.Service.Dtos.Customers.Get;

namespace Library.Service.Services;

public class CustomerService : BaseService<Customer>, ICustomerService
{
    public CustomerService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork, validationService)
    {
    }

    public async Task<CustomerDto?> GetById(string Id)
    {
        var customer = await _unitOfWork.Customers.GetById(Id);

        if (customer is null)
        {
            return null;
        }

        return customer.MapToCustomerDto();
    }

    public async Task<EntityFiltersDto<CustomerDto>> GetAllFilteredCustomers(EntityFiltersDto<CustomerDto> customerFilters)
    {
        var customers = _unitOfWork.Customers.GetAllAsQueryable();
        customerFilters.TotalItems = await customers.CountAsync();

        customers = customers.ApplySearch(customerFilters.SearchString, GetCustomerSearchProperties());
        customers = customers.ApplySort(customerFilters.SortBy, customerFilters.SortOrder, GetCustomerSortDictionary());
        customers = customers.ApplyPagination(customerFilters.PageNumber, customerFilters.PageSize);

        var customersDto = await customers.Select(c => c.MapToCustomerDto()).ToListAsync();

        customerFilters.Entities = customersDto;
        return customerFilters;
    }

    public async Task<Result> Create(CreateCustomerDto createCustomerDto)
    {
        var customerIsNewResult = await _validationService.CustomerIsNew(createCustomerDto.Id);

        if (customerIsNewResult.IsFailure)
        {
            return customerIsNewResult.Error;
        }

        var customer = createCustomerDto.MapToCustomer();

        await _unitOfWork.Customers.Create(customer);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    // Returns a dictionary that we will later use in generic sort method
    private static Dictionary<string, Expression<Func<Customer, object>>> GetCustomerSortDictionary()
    {
        var dict = new Dictionary<string, Expression<Func<Customer, object>>>
        {
            ["Name"] = c => c.Name
        };

        return dict;
    }

    // Returns a function that we will use to search items
    private static Func<Customer, string>[] GetCustomerSearchProperties()
    {
        return
        [
            b => $"{b.Name} {b.Surname}"
        ];
    }
}
