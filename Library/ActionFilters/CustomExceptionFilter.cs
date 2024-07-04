using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Library.Service.Services.Logger;

namespace Library.ActionFilters;

public class CustomExceptionFilter : IExceptionFilter
{
    private readonly ILoggerManager _logger;

    public CustomExceptionFilter(ILoggerManager logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is Exception)
        {
            _logger.LogError($"An exception occured: {context.Exception.Message}");
            context.HttpContext.Session.SetString("ErrorMessage", context.Exception.Message);
            context.Result = new RedirectToActionResult("PageNotFound", "Home", null);
            context.ExceptionHandled = true;
        }
    }
}