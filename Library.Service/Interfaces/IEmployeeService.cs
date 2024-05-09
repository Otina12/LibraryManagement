using Library.Model.Models;
using Library.Service.Dtos;

namespace Library.Service.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<string>> GetAllRolesOfEmployee(string employeeId);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(string employeeId);
    Task DeleteEmployeeAsync(string employeeId);
}
