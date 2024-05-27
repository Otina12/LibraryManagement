using Library.Model.Models.Menu;

namespace Library.Model.Interfaces;

public interface IRoleMenuRepository
{
    Task<IEnumerable<NavigationMenu>> GetMenuItemsForRoleAsync(string roleId);
    Task<IEnumerable<NavigationMenu>> GetMenuItemsForRolesAsync(HashSet<string> roleIds);

}
