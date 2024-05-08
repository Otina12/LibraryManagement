using Library.Service.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Library.Service.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterEmployee(RegisterDto registerDto);
    Task<IdentityResult> LoginEmployee(LoginDto loginDto);
    Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto);
    Task<(IdentityResult, string?)> ForgotPassword(string email); // result and/or token
    Task Logout();
}
