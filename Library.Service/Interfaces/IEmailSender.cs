using Library.Model.Abstractions;
using Library.Model.Models.Email;

namespace Library.Service.Interfaces;

public interface IEmailSender
{
    //Task<Result<bool>> SendResetPasswordEmailAsync(string toEmail, string token, string username);
    Task<Result<bool>> SendEmail<T>(T model, string subject, string token);
}
