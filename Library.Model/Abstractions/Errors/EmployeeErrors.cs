namespace Library.Model.Abstractions.Errors;

public static class EmployeeErrors
{
    public static readonly Error EmployeeNotFound = new("Employee.EmployeeNotFound", "Employee does not exist.");
}
