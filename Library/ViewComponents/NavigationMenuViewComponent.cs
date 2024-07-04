using AutoMapper;
using Library.Service.Interfaces;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;

        public NavigationMenuViewComponent(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var curUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var menuItems = await _serviceManager.NavMenuService.GetMenuItemsOfEmployeeAsync(curUserId);
            var menuVM = _mapper.Map<IEnumerable<NavigationMenuItemViewModel>>(menuItems);
            var menu = GenerateHierarchy(null, menuVM);
            return View(menu);
        }

        public IEnumerable<NavigationMenuItemViewModel> GenerateHierarchy(Guid? parentId, IEnumerable<NavigationMenuItemViewModel> menuVM)
        {
            var children = menuVM
                .Where(x => x.ParentMenuId == parentId)
                .Select(x => new NavigationMenuItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ControllerName = x.ControllerName,
                    ActionName = x.ActionName,
                    Children = GenerateHierarchy(x.Id, menuVM)
                })
                .ToList();

            return children;
        }
    }
}
