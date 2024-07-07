namespace Library.Service.Dtos.Email.Post;

public record EditEmailDto(
    Guid Id,
    string From,
    string Subject,
    string Body
    );
