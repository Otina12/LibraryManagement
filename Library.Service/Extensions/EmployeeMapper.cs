using Library.Model.Models;
using Library.Service.Dtos.Authorization;
using Library.Service.Dtos.Employee;

namespace Library.Service.Extensions;

public static class EmployeeMapper
{
    public static Employee MapToEmployee(this RegisterDto employeeDto)
    {
        var employee = new Employee()
        {
            Name = employeeDto.Name,
            Surname = employeeDto.Surname,
            CreationDate = DateTime.Now,
            UserName = employeeDto.UserName,
            DateOfBirth = new DateTime(employeeDto.Year, employeeDto.Month, employeeDto.Day),
            NormalizedUserName = employeeDto.UserName.ToUpper(),
            Email = employeeDto.Email,
            NormalizedEmail = employeeDto.Email.ToUpper(),
            PhoneNumber = employeeDto.PhoneNumber
        };

        return employee;
    }

    public static EmployeeDto MapToEmployeeDto(this Employee employee)
    {
        var employeeDto = new EmployeeDto(
            employee.Id,
            employee.Name, 
            employee.Surname, 
            employee.UserName!, 
            employee.Email!,
            employee.PhoneNumber!,
            employee.DateOfBirth,
            employee.CreationDate,
            employee.DeleteDate);

        return employeeDto;
    }
}
