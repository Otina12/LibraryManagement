using Library.Model.Abstractions;
using Library.Service.Dtos.Author;

namespace Library.Service.Interfaces;

public interface IAuthorService
{
    // will be uncommented after I refactor Book and BookCopy tables
    //Task<IEnumerable<Book>> GetAllBooksOfAuthor(Guid authorId);
    //Task<int> GetCountOfBooks(Guid authorId);

    Task<IEnumerable<AuthorDto>> GetAllAuthors();
    Task<Result<AuthorDto>> GetAuthorById(Guid id);
    Task<Result> Create(CreateAuthorDto authorDto);
    Task<Result> Update(AuthorDto authorDto);
}
