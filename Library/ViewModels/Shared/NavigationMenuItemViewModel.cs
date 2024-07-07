namespace Library.ViewModels.Shared;

public class NavigationMenuItemViewModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? ParentMenuId { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }
    public IEnumerable<NavigationMenuItemViewModel>? Children { get; set; }
}
