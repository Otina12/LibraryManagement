namespace Library.Model.Models;

public class OriginalBook
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public ICollection<Book> Books { get; } = [];
}
