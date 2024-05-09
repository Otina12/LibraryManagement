using Library.Model.Interfaces;
using Library.Service.Interfaces;
using Library.ViewModels;
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
        public async Task<IActionResult> Index()
        {
            var employees = await _serviceManager.EmployeeService.GetAllEmployeesAsync();
            var employeesAndRoles = new List<EmployeeRolesViewModel>();

            foreach(var employee in employees)
            {
                var employeeVM = new EmployeeRolesViewModel(employee.Id, employee.Name,
                    employee.Surname, employee.Username, employee.Email, employee.PhoneNumber,
                    await _serviceManager.EmployeeService.GetAllRolesOfEmployee(employee.Id));

                employeesAndRoles.Add(employeeVM);
            }

            return View(employeesAndRoles);
        }
    }
}
