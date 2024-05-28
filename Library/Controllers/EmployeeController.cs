using AutoMapper;
using Library.Model.Enums;
using Library.Model.Interfaces;
using Library.Service.Interfaces;
using Library.ViewModels;
using Library.ViewModels.Attributes.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public EmployeeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize]
        [CustomAuthorize("Admin")] // custom attribute to move users to unauthorized page when they don't have a required role
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _serviceManager.EmployeeService.GetAllEmployeesAsync();
            var employeesAndRoles = new List<EmployeeRolesViewModel>();

            foreach (var employeeDto in employees)
            {
                var employeeVM = new EmployeeRolesViewModel(employeeDto,
                    (await _serviceManager.EmployeeService.GetAllRolesOfEmployee(employeeDto.Id)).Value());

                employeesAndRoles.Add(employeeVM);
            }

            return View(employeesAndRoles);
        }

        [Authorize]
        [CustomAuthorize(nameof(Role.Admin))] // custom attribute to move users to unauthorized page when they don't have a required role
        [HttpGet]
        public async Task<IActionResult> Details(string Id)
        {
            var employeeDtoResult = await _serviceManager.EmployeeService.GetEmployeeByIdAsync(Id);
            
            if(employeeDtoResult is null)
                return RedirectToAction("PageNotFound", "Home");

            var employeeAndRoles = new EmployeeRolesViewModel(employeeDtoResult.Value(),
                (await _serviceManager.EmployeeService.GetAllRolesOfEmployee(Id)).Value());

            return View(employeeAndRoles);
        }



        [CustomAuthorize(nameof(Role.Admin))]
        [HttpPost]
        public async Task<IActionResult> ManageRoles(string employeeId, string? roles)
        {
            var updatedRoles = roles?.Split(',').Where(r => r != "Pending")
                .ToArray() ?? [];

            await _serviceManager.EmployeeService.UpdateRolesAsync(employeeId, updatedRoles);
            return RedirectToAction("Details", "Employee", new { id = employeeId });
        }
    }
}
