using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.Publisher.Get;
using Library.Service.Dtos.Publisher.Post;

namespace Library.Service.Interfaces;

public interface IPublisherService : IBaseService<Publisher>
{
    // will be uncommented after I refactor Book and BookCopy tables
    //Task<IEnumerable<Book>> GetAllBooksOfPublisher(Guid publisherId);
    //Task<int> GetCountOfBooks(Guid publisherId);

    Task<EntityFiltersDto<PublisherDto>> GetAllFilteredPublishers(EntityFiltersDto<PublisherDto> publisherFilters);
    Task<IOrderedEnumerable<PublisherIdAndNameDto>> GetAllPublisherIdAndNames(bool includeDeleted = false);
    Task<Result<PublisherDto>> GetPublisherById(Guid id);
    Task<Result> Create(CreatePublisherDto publisherDto);
    Task<Result> Update(PublisherDto publisherDto);
}
