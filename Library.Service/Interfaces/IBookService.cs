using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos.Book;

namespace Library.Service.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooks();
    Task<Result<BookDto>> GetBookById(Guid id);
    Task<Result> CreateBook(CreateBookDto bookDto);
    IEnumerable<BookCopy> CreateBookCopies(Guid bookId, IEnumerable<BookLocationDto> locations);
}
