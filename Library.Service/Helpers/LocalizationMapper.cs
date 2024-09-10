namespace Library.Service.Helpers;

public class LocalizationMapper
{
    private static Dictionary<string, int> LanguageToIdDictionary = new Dictionary<string, int>()
    {
        { "en", 1 },
        { "de", 2 },
        { "ka", 3 }
    };

    public static int LanguageToID(string language)
    {
        var contains = LanguageToIdDictionary.TryGetValue(language, out var id);
        return contains ? id : 1; // default is English (Id = 1)
    }
}
