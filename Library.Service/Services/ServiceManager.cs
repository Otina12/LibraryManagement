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
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IEmailSender> _emailSender;

    // date access services
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<INavMenuService> _menuService;
    private readonly Lazy<IEmailService> _emailService;

    public ServiceManager(IUnitOfWork unitOfWork, UserManager<Employee> userManager,
        SignInManager<Employee> signInManager, IValidationService validationService,
        RoleManager<IdentityRole> roleManager, IMapper mapper, IOptions<MailjetSettings> emailOptions)
    {
        _authService = new Lazy<IAuthService>(() => new AuthService(userManager, signInManager, validationService));
        _emailSender = new Lazy<IEmailSender>(() => new EmailSender(unitOfWork, emailOptions));

        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(unitOfWork, userManager, mapper));
        _menuService = new Lazy<INavMenuService>(() => new NavMenuService(unitOfWork, roleManager, userManager));
        _emailService = new Lazy<IEmailService>(() => new EmailService(unitOfWork));
    }

    public IAuthService AuthService => _authService.Value;
    public IEmailSender EmailSender => _emailSender.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;
    public INavMenuService NavMenuService => _menuService.Value;
    public IEmailService EmailService => _emailService.Value;


}
