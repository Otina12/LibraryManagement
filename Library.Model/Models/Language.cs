namespace Library.Model.Models;

public class Language
{
    public int Id { get; set; }
    public string Code { get; set; } // 2 letters
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<OriginalBookTranslation> OriginalBooks { get; set; } = [];
}
