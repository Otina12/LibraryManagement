using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models;

public class OriginalBookGenre
{
    [ForeignKey("OriginalBook")]
    public Guid OriginalBookId { get; set; }

    [ForeignKey("Genre")]
    public int GenreId { get; set; }

    public OriginalBook OriginalBook { get; set; }
    public Genre Genre { get; set; }
}
