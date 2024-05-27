using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;

        public NavigationMenuViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuItems = await _serviceManager.NavMenuService.GetMenuItemsOfEmployeeAsync(HttpContext.User);
            return View(menuItems);
        }
    }
}
