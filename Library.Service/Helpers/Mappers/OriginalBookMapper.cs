using Library.Model.Models;
using Library.Service.Dtos.OriginalBook.Get;
using Library.Service.Dtos.OriginalBook.Post;

namespace Library.Service.Helpers.Mappers;

public static class OriginalBookMapper
{
    public static OriginalBookDto MapToOriginalBookDto(this OriginalBook originalBook)
    {
        return new OriginalBookDto(
            originalBook.Id,
            originalBook.OriginalPublishYear,
            originalBook.CreationDate,
            originalBook.IsDeleted
            )
        {
            Title = originalBook.Title,
            Description = originalBook.Description,
        };
    }



    public static OriginalBook MapToOriginalBook(this CreateOriginalBookDto createOriginalBookDto)
    {
        return new OriginalBook()
        {
            Title = createOriginalBookDto.EnglishTitle,
            Description = createOriginalBookDto.EnglishDescription,
            OriginalPublishYear = createOriginalBookDto.OriginalPublishYear,
            CreationDate = DateTime.UtcNow
        };
    }

    public static OriginalBook MapToOriginalBook(this EditOriginalBookDto originalBookDto)
    {
        return new OriginalBook()
        {
            Id = originalBookDto.Id,
            Title = originalBookDto.EnglishTitle,
            Description = originalBookDto.EnglishDescription ?? "",
            OriginalPublishYear = originalBookDto.OriginalPublishYear,
            CreationDate = originalBookDto.CreationDate
        };
    }

    public static OriginalBookTranslation MapToTranslation(this CreateOriginalBookTranslationDto translationDto)
    {
        return new OriginalBookTranslation()
        {
            OriginalBookId = translationDto.OriginalBookId,
            LanguageId = translationDto.LanguageId,
            Title = translationDto.Title,
            Description = translationDto.Description
        };
    }
}
