namespace Library.Service.Dtos.Email;

public record EmailToSendDto(
    string Username,
    string Email,
    string Link
);
