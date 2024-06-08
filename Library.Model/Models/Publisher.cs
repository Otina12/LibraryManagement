namespace Library.Model.Models;

public class Publisher : BaseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int YearPublished { get; set; } // when was this publisher company founded

    public ICollection<Book> BooksPublished { get; } = [];
}
