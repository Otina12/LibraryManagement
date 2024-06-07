using System.ComponentModel.DataAnnotations;
namespace Library.Service.Dtos.Authorization;

public record ResetPasswordDto(string Email, string Password, string Token);