using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Shelf> Shelves { get; } = [];
}
