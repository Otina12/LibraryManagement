using Library.Service.Dtos.Employee;

namespace Library.ViewModels.Employees;

public record EmployeeRolesViewModel(
    EmployeeDto Employee,
    IEnumerable<string> Roles
);

