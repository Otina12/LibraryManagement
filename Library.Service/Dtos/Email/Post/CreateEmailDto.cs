namespace Library.Service.Dtos.Email.Post;

public record CreateEmailDto(
    string From,
    string Subject,
    string Body
    );
