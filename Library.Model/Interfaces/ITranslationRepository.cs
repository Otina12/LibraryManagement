using Library.Model.Models;

namespace Library.Data.Repositories;

public interface ITranslationRepository
{
    Task<OriginalBook> TranslateOriginalBook(OriginalBook originalBook, int languageId);
}
