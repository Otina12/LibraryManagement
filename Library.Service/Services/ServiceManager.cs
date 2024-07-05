using Library.Data.Configurations.Variables;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Interfaces;
using Library.Service.Services.Logger;
using Microsoft.AspNetCore.Identity;
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
    private readonly Lazy<IGenreService> _genreService;
    private readonly Lazy<IRoomService> _roomService;
    private readonly Lazy<IShelfService> _shelfService;
    private readonly Lazy<ICustomerService> _customerService;

    public ServiceManager(IUnitOfWork unitOfWork, UserManager<Employee> userManager,
        SignInManager<Employee> signInManager, IValidationService validationService,
        RoleManager<IdentityRole> roleManager, IOptions<MailjetSettings> emailOptions, ILoggerManager logger)
    {
        _authService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, signInManager, validationService, logger));
        _emailSender = new Lazy<IEmailSender>(() => new EmailSender(unitOfWork, emailOptions));

        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(unitOfWork, userManager, validationService));
        _menuService = new Lazy<INavMenuService>(() => new NavMenuService(unitOfWork, roleManager, userManager));
        _emailService = new Lazy<IEmailService>(() => new EmailService(unitOfWork, validationService));
        _publisherService = new Lazy<IPublisherService>(() => new PublisherService(unitOfWork, validationService));
        _authorService = new Lazy<IAuthorService>(() => new AuthorService(unitOfWork, validationService));
        _bookService = new Lazy<IBookService>(() => new BookService(unitOfWork, validationService));
        _genreService = new Lazy<IGenreService>(() => new GenreService(unitOfWork, validationService));
        _roomService = new Lazy<IRoomService>(() => new RoomService(unitOfWork, validationService));
        _shelfService = new Lazy<IShelfService>(() => new ShelfService(unitOfWork, validationService));
        _customerService = new Lazy<ICustomerService>(() => new CustomerService(unitOfWork, validationService));
    }

    public IAuthenticationService AuthService => _authService.Value;
    public IEmailSender EmailSender => _emailSender.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;
    public INavMenuService NavMenuService => _menuService.Value;
    public IEmailService EmailService => _emailService.Value;
    public IPublisherService PublisherService => _publisherService.Value;
    public IAuthorService AuthorService => _authorService.Value;
    public IBookService BookService => _bookService.Value;
    public IGenreService GenreService => _genreService.Value;
    public IRoomService RoomService => _roomService.Value;
    public IShelfService ShelfService => _shelfService.Value;
    public ICustomerService CustomerService => _customerService.Value;
}
