using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Models;
using Library.Service.Dtos.Authorization;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<Employee> _userManager;
    private readonly SignInManager<Employee> _signInManager;
    private readonly IValidationService _validationService;

    public AuthenticationService(UserManager<Employee> userManager, SignInManager<Employee> signInManager,
        IValidationService validationService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _validationService = validationService;
    }

    public async Task<Result> RegisterEmployee(RegisterDto registerDto)
    {
        var supposedUser = await _userManager.FindByEmailAsync(registerDto.Email);

        if (supposedUser is not null) // checking if email is already registered
        {
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
            await _userManager.AddToRoleAsync(employee, "Pending"); // add pending role before admin manually gives the role to employee
            await _signInManager.SignInAsync(employee, false);
            return Result.Success();
        }

        return Result.Failure(new Error(result.Errors.First().Code, result.Errors.First().Description));
    }


    public async Task<Result> LoginEmployee(LoginDto loginDto)
    {
        var supposedUser = await _userManager.FindByEmailAsync(loginDto.Email);

        if (supposedUser is null) // checking if user is registered
        {
            return AuthorizationErrors.EmailNotFound; // gets implicitly converted to Result
        }

        var result = await _signInManager.PasswordSignInAsync(supposedUser, loginDto.Password, false, false);

        if (result.Succeeded)
        {
            return Result.Success();
        }

        return AuthorizationErrors.WrongCredentials; // this will be handled by controller action and then view
    }

    public async Task Logout() // this method is only called when authorized, so no errors can occur
    {
        await _signInManager.SignOutAsync();
    }
    

    public async Task<Result<string>> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            var result = Result.Failure<string>(AuthorizationErrors.EmailNotFound);

            return result;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return Result.Success(token);
    }


    public async Task<Result> ResetPassword(ResetPasswordDto resetPasswordDto) // this only gets called after verifying that user exists
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

        var result = await _userManager.ResetPasswordAsync(user!, resetPasswordDto.Token, resetPasswordDto.Password);

        if (!result.Succeeded)
        {
            return AuthorizationErrors.UknownError;
        }

        await _signInManager.PasswordSignInAsync(user!, resetPasswordDto.Password, false, false);
        return Result.Success();
    }
}
