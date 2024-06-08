namespace Library.Service.Interfaces;

public interface IServiceManager
{
    IAuthorizationService AuthService { get; }
    IEmailSender EmailSender { get; }

    IEmployeeService EmployeeService { get; }
    INavMenuService NavMenuService { get; }
    IEmailService EmailService { get; }
    IPublisherService PublisherService { get; }
    IAuthorService AuthorService { get; }
    IBookService BookService { get; }

}
