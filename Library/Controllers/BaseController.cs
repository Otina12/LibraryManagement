using AutoMapper;
using FluentValidation;
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

        /// <summary>
        /// Handles result depending on if it is failure or success. If it is a success, creates a notification and goes to corresponding controller action
        /// If it is a failure, creates a notification and returns to the view it came from with corresponding viewmodel.
        /// </summary>
        /// <param name="result">Result object needed to check for success/failure</param>
        /// <param name="viewModel">Viewmodel to use in case of failure</param>
        /// <param name="successMessage">Message of notification in case of success</param>
        /// <param name="failureMessage">Message of notification in case of failure</param>
        /// <param name="controllerName">Controller to go to in case of success. Defaults to Home</param>
        /// <param name="actionName">Action to go to in case of success. Defaults to Index</param>
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

        protected Result Validate<T>(T model) where T : class
        {
            var validator = HttpContext.RequestServices.GetService<IValidator<T>>();

            if (validator is not null)
            {
                var result = validator.Validate(model);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return Result.Failure();
                }
            }

            return Result.Success();
        }

        private void CreateNotification(bool isSuccess, string? message)
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
