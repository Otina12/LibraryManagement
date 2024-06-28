using Library.Model.Models.Email;
using Library.Service.Dtos.Email;

namespace Library.Service.Helpers.Extensions;
public static class EmailMapper
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

