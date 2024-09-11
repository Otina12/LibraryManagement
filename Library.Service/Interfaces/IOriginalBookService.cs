using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.OriginalBook.Get;
using Library.Model.Abstractions;
using Library.Service.Dtos.OriginalBook.Post;

namespace Library.Service.Interfaces;

public interface IOriginalBookService : IBaseService<OriginalBook>
{
    Task<Result<OriginalBookDto>> GetOriginalBookById(Guid Id, string culture = "en");
    Task<Result<EditOriginalBookDto>> GetOriginalBookForEditById(Guid Id);
    Task<IEnumerable<OriginalBookDto>> GetAllOriginalBooksSorted(bool includeDeleted = false, string culture = "en");
    Task<EntityFiltersDto<OriginalBookDto>> GetAllFilteredOriginalBooks(EntityFiltersDto<OriginalBookDto> originalBookFilters, string culture = "en");
    Task<Result> Create(CreateOriginalBookDto createOriginalBookDto);
    Task<Result> Update(EditOriginalBookDto editOriginalBookDto);
}
