using Library.Data.Repositories;
using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Publisher;
using Library.Service.Extensions;
using Library.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Service.Services
{
    public class PublisherService : BaseService<Publisher>, IPublisherService
    {
        private readonly IValidationService _validationService;

        public PublisherService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork)
        {
            _validationService = validationService;
        }

        public async Task<IEnumerable<PublisherDto>> GetAllPublishers()
        {
            var publishers = await _unitOfWork.Publishers.GetAll(trackChanges: false);
            var publishersDto = publishers.Select(x => x.MapToPublisherDto()).ToList();

            foreach(var publisherDto in publishersDto)
            {
                await MapBooks(publisherDto);
            }

            return publishersDto;
        }

        public async Task<IOrderedEnumerable<PublisherIdAndNameDto>> GetAllPublisherIdAndNames()
        {
            return (await _unitOfWork.Publishers.GetAll())
                .Select(x => new PublisherIdAndNameDto(x.Id, x.Name))
                .OrderBy(x => x.Name);
        }

        public async Task<Result<PublisherDto>> GetPublisherById(Guid id)
        {
            var publisherExistsResult = await _validationService.PublisherExists(id);

            if (publisherExistsResult.IsFailure)
            {
                return Result.Failure<PublisherDto>(publisherExistsResult.Error);
            }

            var publisherDto = publisherExistsResult.Value().MapToPublisherDto();

            publisherDto.Books = (await _unitOfWork.Books.GetAllBooksOfPublisher(id))
                .Select(x => new BookIdAndTitleDto(x.Id, x.Title)).ToArray();
            return publisherDto;
        }

        public async Task<Result> Create(CreatePublisherDto publisherDto)
        {
            var publisherIsNewResult = await _validationService.PublisherIsNew(publisherDto.Email, publisherDto.Name);

            if (publisherIsNewResult.IsFailure)
            {
                return publisherIsNewResult.Error;
            }

            var publisher = publisherDto.MapToPublisher();

            await _unitOfWork.Publishers.Create(publisher);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> Update(PublisherDto publisherDto)
        {
            var publisherExistsResult = await _validationService.PublisherExists(publisherDto.Id);

            if (publisherExistsResult.IsFailure)
            {
                return Result.Failure(publisherExistsResult.Error);
            }

            var publisher = publisherDto.MapToPublisher();
            publisher.UpdateDate = DateTime.UtcNow;

            _unitOfWork.Publishers.Update(publisher);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        private async Task MapBooks(PublisherDto publisherDto)
        {
            var books = await _unitOfWork.Books.GetAllBooksOfPublisher(publisherDto.Id);
            publisherDto.Books = books.Select(b => new BookIdAndTitleDto(b.Id, b.Title)).ToArray();
        }
    }
}
