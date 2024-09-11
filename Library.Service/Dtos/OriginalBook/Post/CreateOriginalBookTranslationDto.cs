namespace Library.Service.Dtos.OriginalBook.Post;

public record CreateOriginalBookTranslationDto(
    Guid OriginalBookId,
    int LanguageId,
    string Title,
    string Description
    );
