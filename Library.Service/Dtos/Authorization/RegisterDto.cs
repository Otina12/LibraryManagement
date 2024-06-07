namespace Library.Service.Dtos.Authorization;

public record RegisterDto(string Name, string Surname, string Email,
    string UserName, string PhoneNumber, string Password,
    int Year, int Month, int Day);