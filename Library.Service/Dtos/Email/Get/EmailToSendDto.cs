namespace Library.Service.Dtos.Email.Get;

public record EmailToSendDto(
    string Username,
    string Email,
    string Link
);
