using Library.Model.Models.Menu;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Library.Service.Interfaces;

public interface INavMenuService
{
    Task<IEnumerable<NavigationMenu>> GetMenuItemsOfEmployeeAsync(string? userId);
    Task<HashSet<string>> GetRoleIdsOfEmployeeAsync(string userId);
}
