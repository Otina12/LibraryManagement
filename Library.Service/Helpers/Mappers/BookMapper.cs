using Library.Model.Models;
using Library.Service.Dtos.Book;
using System.ComponentModel;
using System.Net;

namespace Library.Service.Helpers.Extensions;

public static class BookMapper
{
    /// <summary>
    /// Maps a Book object into a BookDto object, excluding authors and a publisher.
    /// Use this method to create a basic BookDto instance and add authors and publisher information later.
    /// </summary>
    /// <param name="book">The Book object to map.</param>
    /// <returns>A BookDto object mapped from the Book object.</returns>
    public static BookDto MapToBookDto(this Book book)
    {
        return new BookDto(
            book.Id,
            book.ISBN,
            book.Title,
            book.Edition,
            book.PublishYear,
            book.Quantity)
        {
            AuthorsDto = [],
            PublisherDto = null
        };
    }

    public static BookDetailsDto MapToBookDetailsDto(this Book book)
    {
        return new BookDetailsDto(
            book.Id,
            book.ISBN,
            book.Title,
            book.Edition,
            book.PageCount,
            book.Description,
            book.Quantity,
            book.PublishYear,
            book.IsDeleted)
        {
            Genres = [],
            AuthorsDto = [],
            PublisherDto = null,
            Locations = []
        };
    }

    public static Book MapToBook(this CreateBookDto bookDto)
    {
        return new Book()
        {
            ISBN = bookDto.ISBN,
            Title = bookDto.Title,
            Edition = bookDto.Edition,
            PublishYear = bookDto.PublishYear,
            PageCount = bookDto.PageCount,
            Description = bookDto.Description,
            PublisherId = bookDto.SelectedPublisherId,
            CreationDate = DateTime.UtcNow,
            Quantity = 0
        };
    }

    public static BookCopy MapToBookCopy(this Book book, int roomId, int? shelfId = 0)
    {
        return new BookCopy()
        {
            Status = Model.Enums.Status.Normal,
            BookId = book.Id,
            RoomId = roomId,
            ShelfId = shelfId,
            CreationDate = DateTime.UtcNow
        };
    }
}
