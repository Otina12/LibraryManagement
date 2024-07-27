namespace Library.Model.Models;

public class OriginalBook : BaseModel
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int OriginalPublishYear { get; set; }

    public ICollection<Book> Books { get; } = [];
    public ICollection<OriginalBookGenre> BookGenres { get; } = [];
}
