using Library.Model.Models.Email;
using Library.Service.Dtos.Email;

namespace Library.Service.Extensions;
public static class EmailTemplateMapper
{
    public static EmailModel MapToEmailModel(this CreateEmailDto emailDto)
    {
        return new EmailModel
        {
            From = emailDto.From,
            Subject = emailDto.Subject,
            Body = emailDto.Body,
        };
    }
}

