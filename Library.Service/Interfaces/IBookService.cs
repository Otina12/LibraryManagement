using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos.Book;

namespace Library.Service.Interfaces;

public interface IBookService : IBaseService<Book>
{
    Task<BookListDto> GetAllFilteredBooks(BookListDto bookFilters);
    Task<Result<BookDetailsDto>> GetBookById(Guid id);
    Task<Result> CreateBook(CreateBookDto bookDto);
    Task<Result> UpdateBook(EditBookDto bookDto);
}
