namespace Library.Service.Dtos.Email;

public record CreateEmailDto(
    string From,
    string Subject,
    string Body
    );
