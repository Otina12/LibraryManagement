using Library.Model.Models;
using Library.Service.Dtos;

namespace Library.Service.Extensions;

public static class EmployeeMapper
{
    public static Employee MapToEmployee(this RegisterDto employeeVM)
    {
        var employee = new Employee()
        {
            Name = employeeVM.Name,
            Surname = employeeVM.Surname,
            CreationDate = DateTime.Now,
            UserName = employeeVM.UserName,
            NormalizedUserName = employeeVM.UserName.ToUpper(),
            Email = employeeVM.Email,
            NormalizedEmail = employeeVM.Email.ToUpper(),
            PhoneNumber = employeeVM.PhoneNumber
        };

        return employee;
    }
}
