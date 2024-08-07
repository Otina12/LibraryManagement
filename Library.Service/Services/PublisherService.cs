﻿using Library.Model.Abstractions;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Publisher.Get;
using Library.Service.Dtos.Publisher.Post;
using Library.Service.Helpers;
using Library.Service.Helpers.Extensions;
using Library.Service.Interfaces;
using System.Linq.Expressions;

namespace Library.Service.Services;

public class PublisherService : BaseService<Publisher>, IPublisherService
{
    public PublisherService(IUnitOfWork unitOfWork, IValidationService validationService) : base(unitOfWork, validationService)
    {
    }

    public async Task<EntityFiltersDto<PublisherDto>> GetAllFilteredPublishers(EntityFiltersDto<PublisherDto> publisherFilters)
    {
        var publishers = _unitOfWork.Publishers.GetAllAsQueryable();

        publishers = publishers.IncludeDeleted(publisherFilters.IncludeDeleted);
        publishers = publishers.ApplySearch(publisherFilters.SearchString, GetPublisherSearchProperties());
        publisherFilters.TotalItems = publishers.Count();
        publishers = publishers.ApplySort(publisherFilters.SortBy, publisherFilters.SortOrder, GetPublisherSortDictionary());
        var finalPublishers = publishers.ApplyPagination(publisherFilters.PageNumber, publisherFilters.PageSize).ToList();

        var publishersDto = finalPublishers.Select(p => p.MapToPublisherDto()).ToList();

        foreach (var publisherDto in publishersDto)
        {
            publisherDto.Books = await MapBooks(publisherDto);
        }

        publisherFilters.Entities = publishersDto;
        return publisherFilters;
    }

    public async Task<IOrderedEnumerable<PublisherIdAndNameDto>> GetAllPublisherIdAndNames(bool includeDeleted)
    {
        var publishers = await _unitOfWork.Publishers.GetAll();

        if (!includeDeleted)
        {
            publishers = _unitOfWork.GetBaseModelRepository<Publisher>().FilterOutDeleted(publishers);
        }

        return publishers
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
            .Select(x => new BookIdAndTitleDto(x.Id, x.OriginalBook.Title)).ToArray();
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

    private async Task<BookIdAndTitleDto[]> MapBooks(PublisherDto publisherDto)
    {
        var books = await _unitOfWork.Books.GetAllBooksOfPublisher(publisherDto.Id);
        return books.Select(b => new BookIdAndTitleDto(b.Id, b.OriginalBook.Title)).ToArray();
    }

    // Returns a dictionary that we will later use in generic sort method
    private static Dictionary<string, Expression<Func<Publisher, object>>> GetPublisherSortDictionary()
    {
        var dict = new Dictionary<string, Expression<Func<Publisher, object>>>
        {
            ["Name"] = a => a.Name,
            ["YearPublished"] = b => b.YearPublished
        };

        return dict;
    }

    // Returns a function that we will use to search items
    private static Func<Publisher, string>[] GetPublisherSearchProperties()
    {
        return
        [
            b => b.Name
        ];
    }
}
