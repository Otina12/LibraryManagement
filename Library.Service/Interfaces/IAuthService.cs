using Library.Model.Abstractions;
using Library.Service.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Library.Service.Interfaces;

public interface IAuthService
{
    Task<Result> RegisterEmployee(RegisterDto registerDto);
    Task<Result> LoginEmployee(LoginDto loginDto);
    Task<Result> ResetPassword(ResetPasswordDto resetPasswordDto);
    Task<Result<string>> ForgotPassword(string email); // result and/or token
    Task Logout();
}
