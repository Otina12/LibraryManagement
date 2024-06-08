using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Service.Dtos.Publisher;
using Library.Service.Extensions;
using Library.Service.Interfaces;

namespace Library.Service.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;

        public PublisherService(IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }

        public async Task<IEnumerable<PublisherDto>> GetAllPublishers()
        {
            var publishers = await _unitOfWork.Publishers.GetAll(trackChanges: false);
            var publisherDtos = publishers.Select(x => x.MapToPublisherDto());
            return publisherDtos.ToList();
        }

        public async Task<Result<PublisherDto>> GetPublisherById(Guid id)
        {
            var publisherExistsResult = await _validationService.PublisherExists(id);

            if (publisherExistsResult.IsFailure)
            {
                return Result.Failure<PublisherDto>(publisherExistsResult.Error);
            }

            return publisherExistsResult.Value().MapToPublisherDto();
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

            _unitOfWork.Publishers.Update(publisher);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
