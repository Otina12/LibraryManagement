using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.OriginalBooks;

public class CreateOriginalBookViewModel
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int OriginalPublishYear { get; set; }
    [Display(Name = "Genres")]
    public List<int> SelectedGenreIds { get; set; } = [];
}
