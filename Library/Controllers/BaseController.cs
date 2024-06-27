using AutoMapper;
using Library.Model.Abstractions;
using Library.Service.Interfaces;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IServiceManager _serviceManager;
        protected readonly IMapper _mapper;

        public BaseController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        protected IActionResult HandleResult(Result result, object? viewModel, string successMessage, string failureMessage, string controllerName = "Home", string actionName = "Index")
        {
            if (result.IsFailure)
            {
                CreateFailureNotification(failureMessage);
                return viewModel is null ? View() : View(viewModel);
            }

            CreateSuccessNotification(successMessage);
            return RedirectToAction(actionName, controllerName);
        }

        protected IActionResult HandleResult<T>(Result<T> result, object viewModel, string successMessage, string failureMessage, string controllerName, string actionName)
        {
            return HandleResult((Result)result, viewModel, successMessage, failureMessage, controllerName, actionName);
        }


        protected void CreateNotification(bool isSuccess, string? message)
        {
            TempData["NotificationSuccess"] = isSuccess;
            TempData["NotificationMessage"] = message;
        }

        protected void CreateSuccessNotification(string? message)
        {
            CreateNotification(true, message);
        }

        protected void CreateFailureNotification(string? message)
        {
            CreateNotification(false, message);
        }
    }
}
