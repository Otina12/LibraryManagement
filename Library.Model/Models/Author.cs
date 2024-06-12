namespace Library.Model.Models;

public class Author : BaseModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? Email { get; set; }
    public required string Description { get; set; }
    public int BirthYear { get; set; }
    public int? DeathYear { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; } = [];
}
