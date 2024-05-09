namespace Library.Service.Interfaces;

public interface IServiceManager
{
    IAuthService AuthService { get; }

    IEmployeeService EmployeeService { get; }
}
