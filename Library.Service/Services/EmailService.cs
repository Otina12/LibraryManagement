using Library.Model.Interfaces;
using Library.Model.Models.Email;
using Library.Service.Interfaces;
using Library.Model.Abstractions;
using Library.Service.Helpers.Extensions;
using Library.Service.Dtos.Email.Post;

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
        return await _unitOfWork.EmailTemplates.GetAll(trackChanges: false);
    }

    public async Task<EmailModel?> GetEmail(string subject)
    {
        return await _unitOfWork.EmailTemplates.GetBySubject(subject);
    }

    public async Task<EmailModel?> GetEmail(Guid id)
    {
        return await _unitOfWork.EmailTemplates.GetById(id);
    }


    public async Task<Result> Create(CreateEmailDto emailDto)
    {
        var email = await _unitOfWork.EmailTemplates.GetBySubject(emailDto.Subject);

        if (email is not null)
        {
            return Result.Failure(Error<EmailModel>.AlreadyExists);
        }

        await _unitOfWork.EmailTemplates.Create(emailDto.MapToEmailModel());
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }


    public async Task<Result> Update(EditEmailDto emailDto)
    {
        var email = await _unitOfWork.EmailTemplates.GetById(emailDto.Id);

        if (email is null)
        {
            return Result.Failure(Error<EmailModel>.NotFound);
        }

        email.From = emailDto.From;
        email.Subject = emailDto.Subject;
        email.Body = emailDto.Body;

        _unitOfWork.EmailTemplates.Update(email);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }


    public async Task<Result> Delete(Guid id)
    {
        var email = await _unitOfWork.EmailTemplates.GetById(id);

        if (email is null)
        {
            return Result.Failure(Error<EmailModel>.NotFound);
        }

        Delete(email);
        return Result.Success();
    }

    public void Delete(EmailModel emailModel)
    {
        _unitOfWork.EmailTemplates.Delete(emailModel);
        _unitOfWork.SaveChangesAsync();
    }
}
