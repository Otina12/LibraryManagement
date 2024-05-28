using Library.Data.Configurations.Variables;
using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Service.Dtos.Email;
using Library.Service.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Options;
using System.Web;

namespace Library.Service.Services;

public class EmailSender : IEmailSender
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MailjetSettings _mailjetSettings;

    public EmailSender(IUnitOfWork unitOfWork, IOptions<MailjetSettings> mailjetSettings)
    {
        _unitOfWork = unitOfWork;
        _mailjetSettings = mailjetSettings.Value;
    }


    public async Task<Result<bool>> SendResetPasswordEmailAsync(string toEmail, string token, string username)
    {
        try
        {
            MailjetClient client = new(_mailjetSettings.ApiKey, _mailjetSettings.ApiSecret);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            };

            var emailModel = await _unitOfWork.EmailTemplates.GetBySubject("Reset Password");

            if(emailModel is null)
            {
                return Result.Failure<bool>(EmailErrors.EmailTemplateNotFound);
            }

            // we need to encode token before passing as a string, since '/' will be changed to '%2F', '+' with ' ' and so on...
            var callbackLink = $"https://localhost:44384/Account/ResetPassword?email={toEmail}&token={HttpUtility.UrlEncode(token)}";
            var hrefLink = $"<a href=\"{callbackLink}\">link</a>";
            var emailTemplateDto = new EmailToSendDto(username, toEmail, hrefLink);
            
            string formattedBody = ReplaceTemplate(emailModel.Body, emailTemplateDto);

            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(emailModel.From))
                .WithSubject(emailModel.Subject)
                .WithHtmlPart(formattedBody)
                .WithTo(new SendContact(toEmail))
                .Build();

            var response = await client.SendTransactionalEmailAsync(email);

            var sentSuccessfully = response.Messages[0].Status.Equals("success", StringComparison.CurrentCultureIgnoreCase);

            return sentSuccessfully;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public string ReplaceTemplate<T>(string body, T model)
    {
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            if (property is null) continue;
            string placeholder = $"{{{{{property.Name.ToLower()}}}}}";
            string value = property.GetValue(model)?.ToString() ?? string.Empty;
            body = body.Replace(placeholder, value);
        }
        return body;
    }

}
