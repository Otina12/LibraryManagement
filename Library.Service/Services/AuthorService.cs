using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class AuthorService : BaseService<Author>, IAuthorService
{
    private readonly IValidationService _validationService;

    public AuthorService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork)
    {
        _validationService = validationService;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthors()
    {
        var authors = await _unitOfWork.Authors.GetAll(trackChanges: false);
        var authorsDtos = authors.Select(x => x.MapToAuthorDto()).ToList();

        foreach (var authorDto in authorsDtos)
        {
            await MapBooks(authorDto);
        }

        return authorsDtos;
    }

    public async Task<IOrderedEnumerable<AuthorIdAndNameDto>> GetAllAuthorIdAndNames()
    {
        return (await _unitOfWork.Authors.GetAll())
            .Select(x => new AuthorIdAndNameDto(x.Id, $"{x.Name} {x.Surname}"))
            .OrderBy(x => x.FullName);
    }

    public async Task<Result<AuthorDto>> GetAuthorById(Guid id)
    {
        var authorExistsResult = await _validationService.AuthorExists(id);

        if (authorExistsResult.IsFailure)
        {
            return Result.Failure<AuthorDto>(authorExistsResult.Error);
        }

        var authorDto = authorExistsResult.Value().MapToAuthorDto();
        await MapBooks(authorDto);

        return authorDto;
    }

    public async Task<Result> Create(CreateAuthorDto authorDto)
    {
        var authorIsNewResult = await _validationService.AuthorIsNew(authorDto.Email, authorDto.Name);

        if (authorIsNewResult.IsFailure)
        {
            return authorIsNewResult.Error;
        }

        var author = authorDto.MapToAuthor();

        await _unitOfWork.Authors.Create(author);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> Update(AuthorDto authorDto)
    {
        var authorExistsResult = await _validationService.AuthorExists(authorDto.Id);

        if (authorExistsResult.IsFailure)
        {
            return Result.Failure(authorExistsResult.Error);
        }

        var author = authorDto.MapToAuthor();
        author.UpdateDate = DateTime.UtcNow;

        _unitOfWork.Authors.Update(author);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }


    private async Task MapBooks(AuthorDto authorDto)
    {
        var books = await _unitOfWork.Books.GetAllBooksOfAuthor(authorDto.Id);
        authorDto.Books = books.Select(b => new BookIdAndTitleDto(b.Id, b.Title)).ToArray();
    }
}
