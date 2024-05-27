namespace Library.Service.Interfaces;

public interface IServiceManager
{
    IAuthService AuthService { get; }
    IEmailSender EmailSender { get; }

    IEmployeeService EmployeeService { get; }
    INavMenuService NavMenuService { get; }
    IEmailService EmailService { get; }

}
