namespace Library.Service.Dtos;

public class RegisterDto
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}