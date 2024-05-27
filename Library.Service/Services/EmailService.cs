using Library.Model.Interfaces;
using Library.Model.Models.Email;
using Library.Service.Interfaces;
using Library.Model.Exceptions;
using Library.Service.Dtos.Email;
using Library.Service.Extensions;

namespace Library.Service.Services;

internal class EmailService : IEmailService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmailService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EmailModel>> GetAllTemplates()
    {
        return await _unitOfWork.EmailTemplates.GetAll();
    }

    public async Task<EmailModel?> GetEmail(string subject)
    {
        return await _unitOfWork.EmailTemplates.GetBySubject(subject);
    }

    public async Task<EmailModel?> GetEmail(Guid id)
    {
        return await _unitOfWork.EmailTemplates.GetById(id);
    }


    public async Task Create(CreateEmailDto emailDto)
    {
        var existingSubject = await _unitOfWork.EmailTemplates.GetBySubject(emailDto.Subject);
        if(existingSubject is not null)
        {
            throw new EmailTemplateAlreadyExistsException($"Email template with subject {emailDto.Subject} already exists.");
        }

        await _unitOfWork.EmailTemplates.Create(emailDto.MapToEmailModel());
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task Update(EditEmailDto emailDto)
    {
        var email = await _unitOfWork.EmailTemplates.GetById(emailDto.Id)
            ?? throw new EmailTemplateNotFoundException("Email template was not found.");

        email.From = emailDto.From;
        email.Subject = emailDto.Subject;
        email.Body = emailDto.Body;

        _unitOfWork.EmailTemplates.Update(email);
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task Delete(Guid id)
    {
        var email = await _unitOfWork.EmailTemplates.GetById(id)
            ?? throw new EmailTemplateNotFoundException("Email template was not found.");

        Delete(email); 
    }

    public void Delete(EmailModel emailModel)
    {
        _unitOfWork.EmailTemplates.Delete(emailModel);
        _unitOfWork.SaveChangesAsync();
    }
}
