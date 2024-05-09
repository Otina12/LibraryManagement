using AutoMapper;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Service.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IEmployeeService> _employeeService;

    public ServiceManager(IUnitOfWork unitOfWork, UserManager<Employee> userManager,
        SignInManager<Employee> signInManager, IValidationService validationService, IMapper mapper)
    {
        _authService = new Lazy<IAuthService>(() => new AuthService(userManager, signInManager, validationService));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(unitOfWork, userManager, mapper));

    }

    public IAuthService AuthService => _authService.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;
}
