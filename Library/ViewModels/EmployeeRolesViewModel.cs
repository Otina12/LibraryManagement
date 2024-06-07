using Library.Service.Dtos.Employee;

namespace Library.ViewModels;

public record EmployeeRolesViewModel(
    EmployeeDto Employee,
    IEnumerable<string> Roles
);

