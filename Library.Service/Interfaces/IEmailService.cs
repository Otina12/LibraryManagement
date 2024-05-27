using Library.Model.Exceptions;
using Library.Model.Models.Email;
using Library.Service.Dtos.Email;

namespace Library.Service.Interfaces;

public interface IEmailService
{
    Task<IEnumerable<EmailModel>> GetAllTemplates();
    Task<EmailModel?> GetEmail(string subject);
    Task<EmailModel?> GetEmail(Guid id);
    Task Create(CreateEmailDto emailDto);
    Task Update(EditEmailDto emailDto);
    Task Delete(Guid id);
    void Delete(EmailModel emailModel);
}
