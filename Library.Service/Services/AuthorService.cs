using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Helpers;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Service.Services;

public class AuthorService : BaseService<Author>, IAuthorService
{
    private readonly IValidationService _validationService;

    public AuthorService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork)
    {
        _validationService = validationService;
    }

    public async Task<EntityFiltersDto<AuthorDto>> GetAllFilteredAuthors(EntityFiltersDto<AuthorDto> authorFilters)
    {
        var authors = _unitOfWork.Authors.GetAllAsQueryable();
        authorFilters.TotalItems = await authors.CountAsync();

        authors = authors.ApplySearch(authorFilters.SearchString, GetAuthorSearchProperties());
        authors = authors.ApplySort(authorFilters.SortBy, authorFilters.SortOrder, GetAuthorSortDictionary());
        authors = authors.ApplyPagination(authorFilters.PageNumber, authorFilters.PageSize);

        var authorsDto = await authors.Select(a => a.MapToAuthorDto()).ToListAsync();

        foreach (var authorDto in authorsDto)
        {
            await MapBooks(authorDto);
        }

        authorFilters.Entities = authorsDto;
        return authorFilters;
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

    // Returns a dictionary that we will later use in generic sort method
    private static Dictionary<string, Expression<Func<Author, object>>> GetAuthorSortDictionary()
    {
        var dict = new Dictionary<string, Expression<Func<Author, object>>>
        {
            ["Name"] = a => a.Name,
            ["Period"] = b => b.BirthYear
        };

        return dict;
    }

    // Returns a function that we will use to search items
    private static Func<Author, string>[] GetAuthorSearchProperties()
    {
        return
        [
            b => $"{b.Name} {b.Surname}"
        ];
    }
}
