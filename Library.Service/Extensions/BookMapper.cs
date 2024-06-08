using Library.Model.Models;
using Library.Service.Dtos.Book;
using System.ComponentModel;

namespace Library.Service.Extensions;

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
}
