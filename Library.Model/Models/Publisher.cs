namespace Library.Model.Models;

public class Publisher : BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<BookCopy> BooksPublished { get; } = [];
}
