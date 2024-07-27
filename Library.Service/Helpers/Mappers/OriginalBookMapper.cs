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
            originalBook.Title,
            originalBook.Description,
            originalBook.OriginalPublishYear,
            originalBook.CreationDate,
            originalBook.IsDeleted
            );
    }



    public static OriginalBook MapToOriginalBook(this CreateOriginalBookDto createOriginalBookDto)
    {
        return new OriginalBook()
        {
            Title = createOriginalBookDto.Title,
            Description = createOriginalBookDto.Description,
            OriginalPublishYear = createOriginalBookDto.OriginalPublishYear,
            CreationDate = DateTime.UtcNow
        };
    }

    public static OriginalBook MapToOriginalBook(this OriginalBookDto originalBookDto)
    {
        return new OriginalBook()
        {
            Id = originalBookDto.Id,
            Title = originalBookDto.Title,
            Description = originalBookDto.Description,
            OriginalPublishYear = originalBookDto.OriginalPublishYear,
            CreationDate = originalBookDto.CreationDate
        };
    }
}
