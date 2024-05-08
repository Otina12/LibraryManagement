using AutoMapper;
using FluentValidation;
using Library.Extensions;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Interfaces;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System;

namespace Library.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AccountController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
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

        var result = await _authService.RegisterEmployee(registerDto);

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

        var result = await _authService.LoginEmployee(loginDto);

        return HandleErrors(result, loginVM);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return RedirectToAction("Index", "Home");
    }


    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
    {
        if (!ModelState.IsValid)
        {
            return View(forgotPasswordVM);
        }

        var (result, token) = await _authService.ForgotPassword(forgotPasswordVM.Email);

        if (result.Succeeded)
        {
            return RedirectToAction(nameof(ResetPassword), new { email = forgotPasswordVM.Email, token = token });
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

        var result = await _authService.ResetPassword(resetPasswordDto);

        return HandleErrors(result, resetPasswordVM);
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
