using Library.Model.Models.Email;

namespace Library.Model.Interfaces;

public interface IEmailRepository : IGenericRepository<EmailModel>
{
    Task<EmailModel?> GetBySubject(string subject, bool trackChanges = false);
}
