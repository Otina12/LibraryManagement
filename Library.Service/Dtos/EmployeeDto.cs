namespace Library.Service.Dtos;

public record EmployeeDto(
    string Id, string Name, string Surname, string Username, 
    string Email, string PhoneNumber, DateTime DateOfBirth);
