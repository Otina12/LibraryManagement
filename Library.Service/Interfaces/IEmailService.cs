using Library.Model.Abstractions;
using Library.Model.Models.Email;
using Library.Service.Dtos.Email.Post;

namespace Library.Service.Interfaces;

public interface IEmailService
{
    Task<IEnumerable<EmailModel>> GetAllTemplates();
    Task<EmailModel?> GetEmail(string subject);
    Task<EmailModel?> GetEmail(Guid id);
    Task<Result> Create(CreateEmailDto emailDto);
    Task<Result> Update(EditEmailDto emailDto);
    Task<Result> Delete(Guid id);
    void Delete(EmailModel emailModel);
}
