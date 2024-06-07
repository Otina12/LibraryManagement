namespace Library.Service.Dtos.Employee;

public record UpdateEmployeeDto(
    string Name,
    string Surname,
    string Username,
    string Email,
    string PhoneNumber
    );
