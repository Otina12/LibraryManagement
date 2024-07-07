using AutoMapper;
using Library.Service.Dtos.Authorization;
using Library.Service.Dtos.Email.Get;
using Library.Service.Interfaces;
using Library.ViewModels.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        return HandleResult(result, registerVM, "Your account has been created. Happy managing!", result.Error.Message);
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

        return HandleResult(result, loginVM, "Successfully logged in. Happy managing!", result.Error.Message);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var curUserEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
        await _serviceManager.AuthService.Logout(curUserEmail);
        CreateSuccessNotification("Logged out successfully");
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

        CreateFailureNotification(result.Error.Message);
        return View(forgotPasswordVM);
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

        return HandleResult(result, resetPasswordVM, "Your password has been reset", result.Error.Message);
    }
}
