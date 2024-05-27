using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Library.ActionFilters;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ArgumentException || context.Exception is InvalidOperationException)
        {
            context.Result = new RedirectToActionResult("PageNotFound", "Home", null);
            context.ExceptionHandled = true;
        }
    }
}