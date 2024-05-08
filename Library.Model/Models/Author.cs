namespace Library.Model.Models;

public class Author : BaseModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Description { get; set; }
    public int BirthYear { get; set; }

    public ICollection<Book> Books { get; } = [];
}
