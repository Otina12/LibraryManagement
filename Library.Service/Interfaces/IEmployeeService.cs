using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos.Employee.Get;

namespace Library.Service.Interfaces;

public interface IEmployeeService
{
    Task<Result<IEnumerable<string>>> GetAllRolesOfEmployee(string employeeId);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<Result<EmployeeDto>> GetEmployeeByIdAsync(string employeeId);
    Task<Result> DeactivateEmployeeAsync(string employeeId);
    Task<Result> ReactivateEmployeeAsync(string employeeId);
    Task<Result> RemoveRolesAsync(Employee employee, string[] roles);
    Task<Result> RemoveRolesAsync(string employeeId, string[] roles);

    Task<Result> AddRolesAsync(Employee employee, string[] roles);
    Task<Result> AddRolesAsync(string employeeId, string[] roles);

    Task<Result> UpdateRolesAsync(string employeeId, string[] newRoles);
}
