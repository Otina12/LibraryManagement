namespace Library.ViewModels;

public record EmployeeRolesViewModel(string Id, string Name, string Surname, string UserName,
    string Email, string PhoneNumber, IEnumerable<string> Roles);
