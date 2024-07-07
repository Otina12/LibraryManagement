namespace Library.Service.Dtos.Publisher.Post;

public record CreatePublisherDto(
    string Name,
    string? Email,
    string? PhoneNumber,
    int YearPublished
    );