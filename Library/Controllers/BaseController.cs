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

        protected IActionResult HandleErrors(Result result, object viewModel, NotificationViewModel? notificationVM = null)
        {
            if (result.IsSuccess)
            {
                CreateNotification(true, notificationVM!.SuccessMessage);
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = result.Error.Message;
            //var errorMessages = new List<string>();
            //foreach (var error in result.Errors)
            //{
            //    errorMessages.Add(error.Description);
            //}
            //TempData["ErrorMessages"] = errorMessages;
            CreateNotification(false, notificationVM!.FailureMessage);
            return View(viewModel);
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
