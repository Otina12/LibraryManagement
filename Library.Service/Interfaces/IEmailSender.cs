using Library.Model.Models.Email;

namespace Library.Service.Interfaces;

public interface IEmailSender
{
    Task<bool> SendResetPasswordEmailAsync(string toEmail, string token, string username);
    string ReplaceTemplate<T>(string body, T model); // formatting
}
