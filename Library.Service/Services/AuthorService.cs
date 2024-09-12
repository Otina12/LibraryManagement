using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Helpers;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;
using System.Linq.Expressions;

namespace Library.Service.Services;

public class AuthorService : BaseService<Author>, IAuthorService
{
    public AuthorService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<EntityFiltersDto<AuthorDto>> GetAllFilteredAuthors(EntityFiltersDto<AuthorDto> authorFilters)
    {
        var authors = _unitOfWork.Authors.GetAllAsQueryable();

        authors = authors.IncludeDeleted(authorFilters.IncludeDeleted);
        authors = authors.ApplySearch(authorFilters.SearchString, GetAuthorSearchProperties());
        authorFilters.TotalItems = authors.Count();
        authors = authors.ApplySort(authorFilters.SortBy, authorFilters.SortOrder, GetAuthorSortDictionary());
        var finalAuthors = authors.ApplyPagination(authorFilters.PageNumber, authorFilters.PageSize).ToList();

        var authorsDto = finalAuthors.Select(a => a.MapToAuthorDto()).ToList();

        foreach (var authorDto in authorsDto)
        {
            await MapBooks(authorDto);
        }

        authorFilters.Entities = authorsDto;
        return authorFilters;
    }
    
    public async Task<IOrderedEnumerable<AuthorIdAndNameDto>> GetAllAuthorIdAndNames(bool includeDeleted)
    {
        var authors = await _unitOfWork.Authors.GetAll();
        
        if (!includeDeleted)
        {
            authors = _unitOfWork.GetBaseModelRepository<Author>().FilterOutDeleted(authors);
        }

        return authors.Select(x => new AuthorIdAndNameDto(x.Id, $"{x.Name} {x.Surname}"))
            .OrderBy(x => x.FullName);
    }

    public async Task<Result<AuthorDto>> GetAuthorById(Guid id)
    {
        var author = await _unitOfWork.Authors.GetById(id);

        if (author is null)
        {
            return Result.Failure<AuthorDto>(Error<Author>.NotFound);
        }

        var authorDto = author.MapToAuthorDto();
        await MapBooks(authorDto);

        return authorDto;
    }

    public async Task<Result> Create(CreateAuthorDto authorDto)
    {
        var authorFromDb = authorDto.Email is null ?
                await _unitOfWork.Authors.GetByName(authorDto.Name) :
                await _unitOfWork.Authors.AuthorExists(authorDto.Email, authorDto.Name);

        if (authorFromDb is not null)
        {
            return Result.Failure(Error<Author>.AlreadyExists);
        }

        var author = authorDto.MapToAuthor();

        await _unitOfWork.Authors.Create(author);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> Update(AuthorDto authorDto)
    {
        var authorFromDb = await _unitOfWork.Authors.GetById(authorDto.Id);

        if (authorFromDb is null)
        {
            return Result.Failure<Author>(Error<Author>.NotFound);
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
        authorDto.Books = books.Select(b => new BookIdAndTitleDto(b.Id, b.OriginalBook.Title)).ToArray();
    }

    // Returns a dictionary that we will later use in generic sort method
    private static Dictionary<string, Expression<Func<Author, object>>> GetAuthorSortDictionary()
    {
        var dict = new Dictionary<string, Expression<Func<Author, object>>>
        {
            ["Name"] = c => c.Name,
            ["Period"] = b => b.BirthYear
        };

        return dict;
    }

    // Returns a function that we will use to search items
    private static Func<Author, string>[] GetAuthorSearchProperties()
    {
        return
        [
            a => a.Name,
            a => a.Surname
        ];
    }
}
