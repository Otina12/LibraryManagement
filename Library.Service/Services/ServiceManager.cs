using AutoMapper;
using Library.Data.Configurations.Variables;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Library.Service.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authService;
    private readonly Lazy<IEmailSender> _emailSender;

    // date access services
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<INavMenuService> _menuService;
    private readonly Lazy<IEmailService> _emailService;
    private readonly Lazy<IPublisherService> _publisherService;
    private readonly Lazy<IAuthorService> _authorService;
    private readonly Lazy<IBookService> _bookService;

    public ServiceManager(IUnitOfWork unitOfWork, UserManager<Employee> userManager,
        SignInManager<Employee> signInManager, IValidationService validationService,
        RoleManager<IdentityRole> roleManager, IOptions<MailjetSettings> emailOptions)
    {
        _authService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, signInManager, validationService));
        _emailSender = new Lazy<IEmailSender>(() => new EmailSender(unitOfWork, emailOptions));

        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(unitOfWork, userManager, validationService));
        _menuService = new Lazy<INavMenuService>(() => new NavMenuService(unitOfWork, roleManager, userManager));
        _emailService = new Lazy<IEmailService>(() => new EmailService(unitOfWork, validationService));
        _publisherService = new Lazy<IPublisherService>(() => new PublisherService(unitOfWork, validationService));
        _authorService = new Lazy<IAuthorService>(() => new AuthorService(unitOfWork, validationService));
        _bookService = new Lazy<IBookService>(() => new BookService(unitOfWork, validationService));
    }

    public IAuthenticationService AuthService => _authService.Value;
    public IEmailSender EmailSender => _emailSender.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;
    public INavMenuService NavMenuService => _menuService.Value;
    public IEmailService EmailService => _emailService.Value;
    public IPublisherService PublisherService => _publisherService.Value;
    public IAuthorService AuthorService => _authorService.Value;
    public IBookService BookService => _bookService.Value;
}
