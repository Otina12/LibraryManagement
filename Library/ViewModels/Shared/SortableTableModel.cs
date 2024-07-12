namespace Library.ViewModels.Shared;

public class SortableTableModel
{
    public IEnumerable<object> Items { get; set; } = [];
    public string SearchString { get; set; } = string.Empty;
    public string SortBy { get; set; } = string.Empty;
    public string SortOrder { get; set; } = string.Empty;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool IncludeDeleted { get; set; }
    public int TotalItems { get; set; }
    public List<SortableColumn> Columns { get; set; } = [];
    public string ActionName { get; set; } = string.Empty;
    public string ControllerName { get; set; } = string.Empty;
    public Func<object, string, object> GetPropertyValue { get; set; }
}

public record SortableColumn(string PropertyName, string DisplayName, bool IsSortable);
