using Library.Model.Interfaces;
using Library.Model.Models.Menu;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class RoleMenuRepository : IRoleMenuRepository
{
    private readonly ApplicationDbContext _context;

    public RoleMenuRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NavigationMenu>> GetMenuItemsForRoleAsync(string roleId)
    {
        var menuItems = await _context.RoleMenuPermission
            .Where(m => m.RoleId == roleId)
            .AsNoTracking()
            .Select(m => m.NavigationMenu)
            .ToListAsync();
        return menuItems;
    }

    public async Task<IEnumerable<NavigationMenu>> GetMenuItemsForRolesAsync(HashSet<string> roleIds) // hashset for O(1) search
    {
        var menuItems = await _context.RoleMenuPermission
            .Where(m => roleIds.Contains(m.RoleId))
            .AsNoTracking()
            .Distinct()
            .Select(m => m.NavigationMenu)
            .ToListAsync();
        return menuItems;
    }
}
