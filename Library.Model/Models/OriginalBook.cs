namespace Library.Model.Models;

public class OriginalBook : BaseModel
{
    public Guid Id { get; set; }
    public int OriginalPublishYear { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; } = "";
    public ICollection<Book> Books { get; set; } = [];
    public ICollection<OriginalBookGenre> BookGenres { get; set;  } = [];
    public ICollection<OriginalBookTranslation> Translations { get; set; } = [];
}
