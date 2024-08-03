using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Book.Post;
using Library.Service.Dtos.BookCopy.Post;
using Library.Service.Dtos.OriginalBook.Get;

namespace Library.Service.Interfaces;

public interface IBookService : IBaseService<Book>
{
    Task<Dictionary<OriginalBookIdAndTitle, IEnumerable<BookEditionDto>>> GetAllBookEditions(bool includeDeleted = false);
    Task<EntityFiltersDto<BookDto>> GetAllFilteredBooks(EntityFiltersDto<BookDto> bookFilters);
    Task<Result<BookDetailsDto>> GetBookById(Guid id);
    Task<Result> CreateBook(CreateBookDto bookDto, string creationMessage);
    Task<Result> UpdateBook(EditBookDto bookDto, string creationMessage);
    Task<Result> CreateBookCopies(CreateBookCopiesDto bookCopiesDto);
}
