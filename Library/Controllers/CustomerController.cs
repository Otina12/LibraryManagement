using AutoMapper;
using Library.Service.Dtos;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Customers.Post;
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
    public IActionResult Index(string searchString, string sortBy, string sortOrder, int pageNumber = 1, int pageSize = 10)
    {
        var customerParams = new EntityFiltersDto<CustomerDto>
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var customers = _serviceManager.CustomerService.GetAllFilteredCustomers(customerParams);
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
        var valResult = Validate(createCustomerVM);
        if (valResult.IsFailure)
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
