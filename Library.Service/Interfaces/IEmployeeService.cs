using Library.Model.Models;
using Library.Service.Dtos;

namespace Library.Service.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<string>> GetAllRolesOfEmployee(string employeeId);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(string employeeId);
    Task DeleteEmployeeAsync(string employeeId);

    Task RemoveRolesAsync(Employee employee, string[] roles);
    Task RemoveRolesAsync(string employeeId, string[] roles);

    Task AddRolesAsync(Employee employee, string[] roles);
    Task AddRolesAsync(string employeeId, string[] roles);

    Task UpdateRolesAsync(string employeeId, string[] newRoles);
    Task UpdateRolesAsync(Employee employee, string[] oldRoles, string[] newRoles);
    Task UpdateRolesAsync(string employeeId, string[] oldRoles, string[] newRoles);
}
