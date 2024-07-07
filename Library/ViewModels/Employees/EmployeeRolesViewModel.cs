using Library.Service.Dtos.Employee.Get;

namespace Library.ViewModels.Employees;

public record EmployeeRolesViewModel(
    EmployeeDto Employee,
    IEnumerable<string> Roles
);

