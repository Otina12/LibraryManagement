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
    public IActionResult Index(string searchString, string sortBy, string sortOrder, bool includeDeleted, int pageNumber = 1, int pageSize = 10)
    {
        var customerParams = new EntityFiltersDto<CustomerDto>
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize,
            IncludeDeleted = includeDeleted
        };

        var customers = _serviceManager.CustomerService.GetAllFilteredCustomers(customerParams);
        var cusomtersTable = IndexTables.GetCustomerTable(customers);
        return View(cusomtersTable);
    }

    public async Task<IActionResult> Details(string Id)
    {
        var customerDtoResult = await _serviceManager.CustomerService.GetCustomerById(Id);

        if (customerDtoResult.IsFailure)
        {
            CreateFailureNotification($"Customer with ID: '{Id}' does not exist");
            return RedirectToAction("Index", "Customer");
        }

        return View(customerDtoResult.Value());
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

        var createCustomerDto = _mapper.Map<CreateCustomerDto>(createCustomerVM);
        var result = await _serviceManager.CustomerService.Create(createCustomerDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Customer '{createCustomerVM.Name}' has been created");
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<ActionResult> Edit(string id)
    {
        var customerResult = await _serviceManager.CustomerService.GetCustomerById(id);

        if (customerResult.IsFailure)
            return RedirectToAction("PageNotFound", "Home");

        var editViewModel = _mapper.Map<CustomerViewModel>(customerResult.Value());

        return PartialView("_EditPartial", editViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CustomerViewModel customerVM)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_EditPartial", customerVM);
        }

        var customerDto = _mapper.Map<CustomerDto>(customerVM);
        var result = await _serviceManager.CustomerService.Update(customerDto);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification($"Customer '{customerVM.Name}' has been updated");
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _serviceManager.CustomerService.Deactivate(stringId: id);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Customer has been deactivated successfully");
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Renew(string id)
    {
        var result = await _serviceManager.CustomerService.Reactivate(stringId: id);

        if (result.IsFailure)
        {
            CreateFailureNotification(result.Error.Message);
            return Json(new { success = false });
        }

        CreateSuccessNotification("Customer has been reactivated successfully");
        return Json(new { success = true });
    }
}
