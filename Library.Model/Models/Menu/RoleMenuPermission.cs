using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Model.Models.Menu;

[PrimaryKey(nameof(RoleId), nameof(NavigationMenuId))]
public class RoleMenuPermission
{

    [ForeignKey("Role")]
    public string RoleId { get; set; }

    [ForeignKey("NavigationMenu")]
    public Guid NavigationMenuId { get; set; }

    // navigation props
    public IdentityRole Role { get; set; }
    public NavigationMenu NavigationMenu { get; set; }
}
