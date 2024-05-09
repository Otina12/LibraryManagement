using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Extensions;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Service.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<Employee> _userManager;
    private readonly SignInManager<Employee> _signInManager;
    private readonly IValidationService _validationService;

    public AuthService(UserManager<Employee> userManager, SignInManager<Employee> signInManager,
        IValidationService validationService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _validationService = validationService;
    }

    public async Task<IdentityResult> RegisterEmployee(RegisterDto registerDto)
    {
        var supposedUser = await _userManager.FindByEmailAsync(registerDto.Email);

        if (supposedUser is not null) // checking if email is already registered
        {
            return IdentityResult.Failed(new IdentityError // this will be handled by controller action and then view
            {
                Code = "EmployeeAlreadyExists",
                Description = $"Email '{registerDto.Email}' is already in use"
            });
        }

        if(!_validationService.BirthdayIsValid(registerDto.Year, registerDto.Month, registerDto.Day))
        {
            return IdentityResult.Failed(new IdentityError // this will be handled by controller action and then view
            {
                Code = "BithdayNotValid",
                Description = $"Please, enter a valid date of birth"
            });
        }

        var employee = registerDto.MapToEmployee();
        var result = await _userManager.CreateAsync(employee, registerDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(employee, "Employee"); // add normal employee role
            await _signInManager.SignInAsync(employee, false);
            return IdentityResult.Success;
        }

        return result;

    }


    public async Task<IdentityResult> LoginEmployee(LoginDto loginDto)
    {
        var supposedUser = await _userManager.FindByEmailAsync(loginDto.Email);

        if (supposedUser is null) // checking if user is registered
        {
            return IdentityResult.Failed(new IdentityError // this will be handled by controller action and then view
            {
                Code = "EmailIsNotRegistered",
                Description = $"Employee with email '{loginDto.Email}' does not exist"
            });
        }

        var result = await _signInManager.PasswordSignInAsync(supposedUser, loginDto.Password, false, false);

        if (result.Succeeded)
        {
            return IdentityResult.Success;
        }

        return IdentityResult.Failed(new IdentityError // this will be handled by controller action and then view
        {
            Code = "WrongCredentials",
            Description = $"Wrong credentials. Please try again"
        });
    }

    public async Task Logout() // this method is only called when authorized, so no errors can occur
    {
        await _signInManager.SignOutAsync();
    }
    

    public async Task<(IdentityResult, string?)> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            var result = IdentityResult.Failed(new IdentityError
            {
                Code = "EmailNotFound",
                Description = $"Email not found. Please try again"
            });

            return (result, null);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return (IdentityResult.Success, token);
    }

    public async Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto) // this only gets called after verifying that user exists
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

        var result = await _userManager.ResetPasswordAsync(user!, resetPasswordDto.Token, resetPasswordDto.Password);

        if (!result.Succeeded)
        {
            return IdentityResult.Failed(new IdentityError // this will be handled by controller action and then view
            {
                Code = "ErrorOccured",
                Description = $"An error occured. Please try again later"
            });
        }

        await _signInManager.PasswordSignInAsync(user!, resetPasswordDto.Password, false, false);
        return IdentityResult.Success;
    }
}
