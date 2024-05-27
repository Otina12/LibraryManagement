using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library.Model.Models.Menu;

public class NavigationMenu
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; }

    [ForeignKey("ParentNavigationMenu")]
    public Guid? ParentMenuId { get; set; }

    public virtual NavigationMenu ParentNavigationMenu { get; set; }

    public string ControllerName { get; set; }

    public string ActionName { get; set; }

    [NotMapped]
    public bool Permitted { get; set; }
}
