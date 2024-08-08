using Library.Model.Models;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos;
using Library.Service.Dtos.OriginalBook.Get;
using Library.Model.Abstractions;
using Library.Service.Dtos.OriginalBook.Post;
using Library.Model.Models.Report;
using Library.Service.Dtos.Report;

namespace Library.Service.Interfaces;

public interface IOriginalBookService : IBaseService<OriginalBook>
{
    Task<Result<OriginalBookDto>> GetOriginalBookById(Guid Id);
    IEnumerable<OriginalBookDto> GetAllOriginalBooksSorted(bool includeDeleted = false);
    Task<EntityFiltersDto<OriginalBookDto>> GetAllFilteredOriginalBooks(EntityFiltersDto<OriginalBookDto> originalBookFilters);
    Task<Result> Create(CreateOriginalBookDto createOriginalBookDto);
    Task<Result> Update(OriginalBookDto originalBookDto);
}
