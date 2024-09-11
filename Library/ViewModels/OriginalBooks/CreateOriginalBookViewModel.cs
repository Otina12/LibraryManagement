using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.OriginalBooks;

public class CreateOriginalBookViewModel
{
    public string EnglishTitle { get; set; } = string.Empty;
    public string EnglishDescription { get; set; } = string.Empty;
    public string GermanTitle { get; set; } = string.Empty;
    public string GermanDescription { get; set; } = string.Empty;
    public string GeorgianTitle { get; set; } = string.Empty;
    public string GeorgianDescription { get; set; } = string.Empty;
    public int OriginalPublishYear { get; set; }
    [Display(Name = "Genres")]
    public List<int> SelectedGenreIds { get; set; } = [];
}
