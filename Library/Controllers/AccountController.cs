using AutoMapper;
using Library.Model.Abstractions;
using Library.Service.Dtos.Authorization;
using Library.Service.Dtos.Email;
using Library.Service.Interfaces;
using Library.ViewModels;
using Library.ViewModels.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class AccountController : BaseController
{
    public AccountController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerVM)
    {
        if (!ModelState.IsValid)
        {
            return View(registerVM);
        }

        var registerDto = _mapper.Map<RegisterDto>(registerVM); // we map to Dto to not to expose viewmodels to service

        var result = await _serviceManager.AuthService.RegisterEmployee(registerDto);

        return HandleErrors(result, registerVM, new NotificationViewModel("Your account has been created. Happy managing!", "Registration failed. Try again later"));
    }


    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginVM)
    {
        if (!ModelState.IsValid)
        {
            return View(loginVM);
        }
        
        var loginDto = _mapper.Map<LoginDto>(loginVM);

        var result = await _serviceManager.AuthService.LoginEmployee(loginDto);

        return HandleErrors(result, loginVM, new NotificationViewModel("Successfully logged in. Happy managing!", "Login failed. Try again later"));
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _serviceManager.AuthService.Logout();
        CreateNotification(true, "Logged out successfully");
        return RedirectToAction("Index", "Home");
    }


    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult?> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
    {
        if (!ModelState.IsValid)
        {
            return View(forgotPasswordVM);
        }

        var result = await _serviceManager.AuthService.ForgotPassword(forgotPasswordVM.Email);

        if (result.IsSuccess)
        {
            // we send reset password email to the user with the link
            await _serviceManager.EmailSender.SendEmail(
                new EmailToSendDto(
                    forgotPasswordVM.Email.Split('@')[0],
                    forgotPasswordVM.Email,
                    "Reset Password"),
                "Reset Password",
                result.Value());
            
            return View(forgotPasswordVM);
        }

        return HandleErrors(result, forgotPasswordVM);
    }


    public IActionResult ResetPassword(string email, string token)
    {
        var model = new ResetPasswordViewModel
        {
            Email = email,
            Token = token
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
    {
        if (!ModelState.IsValid)
        {
            return View(resetPasswordVM);
        }

        var resetPasswordDto = _mapper.Map<ResetPasswordDto>(resetPasswordVM);

        var result = await _serviceManager.AuthService.ResetPassword(resetPasswordDto);

        return HandleErrors(result, resetPasswordVM, new NotificationViewModel("Your password has been reset", "Unknown error occured while reseting the password"));
    }
}
