using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Menu;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Library.Service.Services;

public class NavMenuService : INavMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<Employee> _userManager;

    public NavMenuService(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager,
        UserManager<Employee> userManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userManager = userManager;
    }


    // this method will help us determine which menu items to display to the employee
    public async Task<IEnumerable<NavigationMenu>> GetMenuItemsOfEmployeeAsync(string? userId)
    {
        // when employee is not authenticated, return defaults (nothing in this case)
        if (userId is null)
        {
            return new List<NavigationMenu>();
        }

        // when employee is authenticated
        var roleIds = await GetRoleIdsOfEmployeeAsync(userId);
        var menuItems = await _unitOfWork.RoleMenus.GetMenuItemsForRolesAsync(roleIds);

        return menuItems;
    }

    public async Task<HashSet<string>> GetRoleIdsOfEmployeeAsync(string userId)
    {
        var employee = await _unitOfWork.Employees.GetById(new Guid(userId!), trackChanges: false);

        // select role names (strings)
        var roleNames = await _userManager.GetRolesAsync(employee!);

        if(!roleNames.Any())
            return [];

        var roleIds = new HashSet<string>(); // get roleIds from names
        
        foreach(var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            roleIds.Add(role!.Id);
        }

        return roleIds; // is never [] because default 'pending' role is always present
    }
}
