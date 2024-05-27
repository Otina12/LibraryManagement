using Library.Model.Models.Menu;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Library.Service.Interfaces;

public interface INavMenuService
{
    Task<IEnumerable<NavigationMenu>> GetMenuItemsOfEmployeeAsync(ClaimsPrincipal? claimsPrincipal);
    Task<HashSet<string>> GetRoleIdsOfEmployeeAsync(ClaimsPrincipal claimsPrincipal);
}
