using System.ComponentModel.DataAnnotations;
namespace Library.Service.Dtos;

public record ResetPasswordDto(string Email, string Password, string Token);