using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Models;
using Library.Service.Dtos.Authorization;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;
using Library.Service.Services.Logger;
using Microsoft.AspNetCore.Identity;

namespace Library.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<Employee> _userManager;
    private readonly SignInManager<Employee> _signInManager;
    private readonly IValidationService _validationService;
    private readonly ILoggerManager _loggerManager;

    public AuthenticationService(UserManager<Employee> userManager, SignInManager<Employee> signInManager,
        IValidationService validationService, ILoggerManager loggerManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _validationService = validationService;
        _loggerManager = loggerManager;
    }

    public async Task<Result> RegisterEmployee(RegisterDto registerDto)
    {
        var supposedUser = await _userManager.FindByEmailAsync(registerDto.Email);

        if (supposedUser is not null) // checking if email is already registered
        {
            _loggerManager.LogWarning($"User tried to register with email '{registerDto.Email}' that already exists");
            return AuthorizationErrors.EmployeeAlreadyExists;
        }

        if(!_validationService.BirthdayIsValid(registerDto.Year, registerDto.Month, registerDto.Day))
        {
            return AuthorizationErrors.InvalidBirthday;
        }

        var employee = registerDto.MapToEmployee();
        var result = await _userManager.CreateAsync(employee, registerDto.Password);

        if (result.Succeeded)
        {
            _loggerManager.LogInfo($"User '{registerDto.Email}' registered successfully");
            await _userManager.AddToRoleAsync(employee, "Pending"); // add pending role before admin manually gives the role to employee
            await _signInManager.SignInAsync(employee, false);
            return Result.Success();
        }

        return Result.Failure(new Error(result.Errors.First().Code, result.Errors.First().Description));
    }


    public async Task<Result> LoginEmployee(LoginDto loginDto)
    {
        _loggerManager.LogInfo($"Trying to log in user with email '{loginDto.Email}'");
        var supposedUser = await _userManager.FindByEmailAsync(loginDto.Email);

        if (supposedUser is null) // checking if user is registered
        {
            _loggerManager.LogWarning($"User with email '{loginDto.Email}' was not found");
            return AuthorizationErrors.EmailNotFound; // gets implicitly converted to Result
        }

        var result = await _signInManager.PasswordSignInAsync(supposedUser, loginDto.Password, false, false);

        if (result.Succeeded)
        {
            _loggerManager.LogInfo($"Logged in user with email '{loginDto.Email}'");
            return Result.Success();
        }

        _loggerManager.LogWarning($"Wrong credentials entered for user '{loginDto.Email}'");
        return AuthorizationErrors.WrongCredentials; // this will be handled by controller action and then view
    }

    public async Task Logout(string? email) // this method is only called when authorized, so no errors can occur
    {
        _loggerManager.LogWarning($"User '{email}' logged out");
        await _signInManager.SignOutAsync();
    }
    

    public async Task<Result<string>> ForgotPassword(string email)
    {
        _loggerManager.LogInfo($"Trying to generate ForgotPassword token for user '{email}'");
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            _loggerManager.LogWarning($"Could not find user {email} to reset password");
            var result = Result.Failure<string>(AuthorizationErrors.EmailNotFound);
            return result;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        _loggerManager.LogInfo($"Generated token for user '{email}'");
        return Result.Success(token);
    }


    public async Task<Result> ResetPassword(ResetPasswordDto resetPasswordDto) // this only gets called after verifying that user exists
    {
        _loggerManager.LogInfo($"Trying to reset password for user '{resetPasswordDto.Email}'");
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

        var result = await _userManager.ResetPasswordAsync(user!, resetPasswordDto.Token, resetPasswordDto.Password);

        if (!result.Succeeded)
        {
            _loggerManager.LogWarning($"An error occured while changing password of user '{resetPasswordDto.Email}'");
            return AuthorizationErrors.UknownError;
        }

        _loggerManager.LogInfo($"Changed password of user '{resetPasswordDto.Email}'");
        await _signInManager.PasswordSignInAsync(user!, resetPasswordDto.Password, false, false);
        return Result.Success();
    }
}
