using Library.Model.Abstractions;
using Library.Service.Dtos.Book;

namespace Library.Service.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooks();
    Task<Result<BookDto>> GetBookById(Guid id);
}
