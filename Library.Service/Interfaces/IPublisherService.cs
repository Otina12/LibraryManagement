using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Service.Dtos.Publisher;

namespace Library.Service.Interfaces;

public interface IPublisherService
{
    // will be uncommented after I refactor Book and BookCopy tables
    //Task<IEnumerable<Book>> GetAllBooksOfPublisher(Guid publisherId);
    //Task<int> GetCountOfBooks(Guid publisherId);

    Task<IEnumerable<PublisherDto>> GetAllPublishers();
    Task<Result<PublisherDto>> GetPublisherById(Guid id);
    Task<Result> Create(CreatePublisherDto publisherDto);
    Task<Result> Update(PublisherDto publisherDto);
}
