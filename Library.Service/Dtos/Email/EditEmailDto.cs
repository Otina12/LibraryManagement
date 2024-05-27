namespace Library.Service.Dtos.Email;

public record EditEmailDto(
    Guid Id,
    string From,
    string Subject,
    string Body
    );
