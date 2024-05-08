namespace Library.Model.Models;

public class Book : BaseModel
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int PublishYear { get; set; }

    // "skip" navigation properties
    public ICollection<Author> Authors { get; } = [];
    public ICollection<Genre> Genres { get; } = [];
    public ICollection<BookCopy> BookCopies { get; } = [];

}
