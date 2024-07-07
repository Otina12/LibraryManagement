namespace Library.Service.Dtos.Employee.Get;

public record EmployeeDto(
    string Id,
    string Name,
    string Surname,
    string Username,
    string Email,
    string PhoneNumber,
    DateTime DateOfBirth,
    DateTime CreateDate,
    DateTime? DeleteDate)
{
    public bool IsEmployed => DeleteDate is null;
}
