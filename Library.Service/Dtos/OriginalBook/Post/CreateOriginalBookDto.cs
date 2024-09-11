namespace Library.Service.Dtos.OriginalBook.Post;

public record CreateOriginalBookDto(
    int OriginalPublishYear,
    List<int> SelectedGenreIds)
{
    public string EnglishTitle { get; set; } = string.Empty;
    public string EnglishDescription { get; set; } = string.Empty;
    public string GermanTitle { get; set; } = string.Empty;
    public string GermanDescription { get; set; } = string.Empty;
    public string GeorgianTitle { get; set; } = string.Empty;
    public string GeorgianDescription { get; set; } = string.Empty;

    public (string title, string description) GetTranslation(int languageId)
    {
        return languageId switch
        {
            1 => (EnglishTitle, EnglishDescription),
            2 => (GermanTitle, GermanDescription),
            3 => (GeorgianTitle, GeorgianDescription),
            _ => (string.Empty, string.Empty)
        };
    }
}
