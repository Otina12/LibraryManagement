using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Publisher;
using Library.Service.Extensions;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class AuthorService : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public AuthorService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthors()
    {
        var authors = await _unitOfWork.Authors.GetAll(trackChanges: false);
        var authorsDtos = authors.Select(x => x.MapToAuthorDto());
        return authorsDtos.ToList();
    }

    public async Task<Result<AuthorDto>> GetAuthorById(Guid id)
    {
        var authorExistsResult = await _validationService.AuthorExists(id);

        if (authorExistsResult.IsFailure)
        {
            return Result.Failure<AuthorDto>(authorExistsResult.Error);
        }

        return authorExistsResult.Value().MapToAuthorDto();
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

        _unitOfWork.Authors.Update(author);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
