namespace Library.Model.Models;

public class OriginalBookTranslation
{
    public Guid OriginalBookId { get; set; }
    public int LanguageId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public OriginalBook OriginalBook { get; set; }
    public Language Language { get; set; }
}
