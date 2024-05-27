using Library.Service.Dtos;

namespace Library.ViewModels;

public record EmployeeRolesViewModel(
    EmployeeDto Employee,
    IEnumerable<string> Roles
);

