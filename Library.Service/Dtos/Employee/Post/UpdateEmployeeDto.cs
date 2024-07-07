namespace Library.Service.Dtos.Employee.Post;

public record UpdateEmployeeDto(
    string Name,
    string Surname,
    string Username,
    string Email,
    string PhoneNumber
    );
