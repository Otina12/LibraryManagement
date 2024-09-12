﻿using Library.Data.Configurations.Variables;
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
    private readonly Lazy<IOriginalBookService> _originalBookService;
    private readonly Lazy<IBookService> _bookService;
    private readonly Lazy<IGenreService> _genreService;
    private readonly Lazy<IRoomService> _roomService;
    private readonly Lazy<IShelfService> _shelfService;
    private readonly Lazy<ICustomerService> _customerService;
    private readonly Lazy<IReservationService> _reservationService;
    private readonly Lazy<IBookCopyLogService> _bookCopyLogService;
    private readonly Lazy<IReportService> _reportService;

    public ServiceManager(IUnitOfWork unitOfWork, UserManager<Employee> userManager,
        SignInManager<Employee> signInManager, IValidationService validationService,
        RoleManager<IdentityRole> roleManager, IOptions<MailjetSettings> emailOptions, ILoggerManager logger, IGenericRepository genericRepository)
    {
        _authService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, signInManager, validationService, logger));
        _emailSender = new Lazy<IEmailSender>(() => new EmailSender(unitOfWork, emailOptions));

        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(unitOfWork, userManager));
        _menuService = new Lazy<INavMenuService>(() => new NavMenuService(unitOfWork, roleManager, userManager));
        _emailService = new Lazy<IEmailService>(() => new EmailService(unitOfWork));
        _publisherService = new Lazy<IPublisherService>(() => new PublisherService(unitOfWork));
        _authorService = new Lazy<IAuthorService>(() => new AuthorService(unitOfWork));
        _originalBookService = new Lazy<IOriginalBookService>(() => new OriginalBookService(unitOfWork));
        _bookService = new Lazy<IBookService>(() => new BookService(unitOfWork));
        _genreService = new Lazy<IGenreService>(() => new GenreService(unitOfWork));
        _roomService = new Lazy<IRoomService>(() => new RoomService(unitOfWork));
        _shelfService = new Lazy<IShelfService>(() => new ShelfService(unitOfWork));
        _customerService = new Lazy<ICustomerService>(() => new CustomerService(unitOfWork));
        _reservationService = new Lazy<IReservationService>(() => new ReservationService(unitOfWork, logger));
        _bookCopyLogService = new Lazy<IBookCopyLogService>(() => new BookCopyLogService(unitOfWork));
        _reportService = new Lazy<IReportService>(() => new ReportService(genericRepository));
    }

    public IAuthenticationService AuthService => _authService.Value;
    public IEmailSender EmailSender => _emailSender.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;
    public INavMenuService NavMenuService => _menuService.Value;
    public IEmailService EmailService => _emailService.Value;
    public IPublisherService PublisherService => _publisherService.Value;
    public IAuthorService AuthorService => _authorService.Value;
    public IOriginalBookService OriginalBookService => _originalBookService.Value;
    public IBookService BookService => _bookService.Value;
    public IGenreService GenreService => _genreService.Value;
    public IRoomService RoomService => _roomService.Value;
    public IShelfService ShelfService => _shelfService.Value;
    public ICustomerService CustomerService => _customerService.Value;
    public IReservationService ReservationService => _reservationService.Value;
    public IBookCopyLogService BookCopyLogService => _bookCopyLogService.Value;
    public IReportService ReportService => _reportService.Value;
}
