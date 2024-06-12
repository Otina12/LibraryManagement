using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos.Employee;

namespace Library.Service.Interfaces;

public interface IEmployeeService
{
    Task<Result<IEnumerable<string>>> GetAllRolesOfEmployee(string employeeId);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<Result<EmployeeDto>> GetEmployeeByIdAsync(string employeeId);
    Task<Result> DeleteEmployeeAsync(string employeeId);
    Task<Result> RenewEmployeeAsync(string employeeId);
    Task<Result> RemoveRolesAsync(Employee employee, string[] roles);
    Task<Result> RemoveRolesAsync(string employeeId, string[] roles);

    Task<Result> AddRolesAsync(Employee employee, string[] roles);
    Task<Result> AddRolesAsync(string employeeId, string[] roles);

    Task<Result> UpdateRolesAsync(string employeeId, string[] newRoles);
    Task<Result> UpdateRolesAsync(Employee employee, string[] oldRoles, string[] newRoles);
}
