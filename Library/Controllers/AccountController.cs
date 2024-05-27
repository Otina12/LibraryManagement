using AutoMapper;
using Library.Service.Dtos;
using Library.Service.Interfaces;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class AccountController : Controller
{
    private readonly IServiceManager _serviceManager;
    private readonly IMapper _mapper;

    public AccountController(IServiceManager serviceManager, IMapper mapper)
    {
        _serviceManager = serviceManager;
        _mapper = mapper;
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

        return HandleErrors(result, registerVM);
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

        return HandleErrors(result, loginVM);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _serviceManager.AuthService.Logout();
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

        var (result, token) = await _serviceManager.AuthService.ForgotPassword(forgotPasswordVM.Email);

        if (result.Succeeded)
        {
            // we send reset password email to the user with the link
            await _serviceManager.EmailSender.SendResetPasswordEmailAsync(forgotPasswordVM.Email, token!, "giorgi");
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

        return HandleErrors(result, resetPasswordVM);
    }


    [HttpGet]
    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }


    private IActionResult HandleErrors(IdentityResult result, object viewModel)
    {
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        var errorMessages = new List<string>();
        foreach (var error in result.Errors)
        {
            errorMessages.Add(error.Description);
        }
        TempData["ErrorMessages"] = errorMessages;

        return View(viewModel);
    }
}
