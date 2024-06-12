using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Library.Attributes.Authorization
{
    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _roles;

        public CustomAuthorizeFilter(string roles)
        {
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var curUser = context.HttpContext.User;

            if (!curUser.Identity!.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (!_roles.Split(',').Any(role => curUser.IsInRole(role))) // check if user has required roles
            {
                // he doesn't. Now check if it's his own profile page
                if (context.RouteData.Values.TryGetValue("id", out var idValue) && idValue is not null)
                {
                    var idFromRoute = idValue.ToString();
                    var currentUserId = curUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (idFromRoute == currentUserId)
                    {
                        await Task.CompletedTask;
                        return;
                    }
                }

                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
            }

            await Task.CompletedTask;
            return;
        }
    }
}
