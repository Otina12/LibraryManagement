using Library.Model.Models;
using Library.Service.Dtos.Publisher;

namespace Library.Service.Extensions;

public static class PublisherMapper
{
    public static PublisherDto MapToPublisherDto(this Publisher publisher)
    {
        return new PublisherDto(
            publisher.Id,
            publisher.Name,
            publisher.Email,
            publisher.PhoneNumber,
            publisher.YearPublished,
            0,
            publisher.CreationDate
        );
    }

    public static Publisher MapToPublisher(this CreatePublisherDto publisherDto)
    {
        return new Publisher()
        {
            Name = publisherDto.Name,
            Email = publisherDto.Email,
            PhoneNumber = publisherDto.PhoneNumber,
            YearPublished = publisherDto.YearPublished,
            CreationDate = DateTime.UtcNow
        };
    }

    public static Publisher MapToPublisher(this PublisherDto publisherDto)
    {
        return new Publisher()
        {
            Id = publisherDto.Id,
            Name = publisherDto.Name,
            Email = publisherDto.Email,
            PhoneNumber = publisherDto.PhoneNumber,
            YearPublished = publisherDto.YearPublished,
            CreationDate = publisherDto.CreationDate
        };
    }
}
