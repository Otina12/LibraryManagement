namespace Library.Service.Dtos.Publisher;

public record CreatePublisherDto(
    string Name,
    string? Email,
    string? PhoneNumber,
    int YearPublished
    );