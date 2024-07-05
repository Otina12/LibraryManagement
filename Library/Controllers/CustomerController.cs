using AutoMapper;
using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Customers;
using Library.Service.Interfaces;
using Library.ViewModels.Customers;
using Library.ViewSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class CustomerController : BaseController
{
    public CustomerController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString, string sortBy, string sortOrder, int pageNumber = 1, int pageSize = 10)
    {
        var customerParams = new EntityFiltersDto<CustomerDto>
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var customers = await _serviceManager.CustomerService.GetAllFilteredCustomers(customerParams);
        var cusomtersTable = IndexTables.GetCustomerTable(customers);
        return View(cusomtersTable);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new CreateCustomerViewModel();
        return PartialView("_CreatePartial", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerViewModel createCustomerVM)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_CreatePartial", createCustomerVM);
        }

        var createAuthorDto = _mapper.Map<CreateCustomerDto>(createCustomerVM);
        var result = await _serviceManager.CustomerService.Create(createAuthorDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Customer {createCustomerVM.Name} has been created");
        return Json(new { success = true });
    }
}
