using Library.Model.Interfaces;
using Library.Model.Models.Email;
using Library.Service.Interfaces;
using Library.Service.Dtos.Email;
using Library.Service.Extensions;
using Library.Model.Abstractions.Errors;
using Library.Model.Abstractions;
using Library.Model.Models;

namespace Library.Service.Services;

internal class EmailService : IEmailService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public EmailService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
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


    public async Task<Result> Create(CreateEmailDto emailDto)
    {
        var emailExistsResult = await _validationService.EmailTemplateExists(emailDto.Subject);
        if (emailExistsResult.IsFailure)
        {
            return Result.Failure(EmailErrors.EmailTemplateNotFound);
        }

        await _unitOfWork.EmailTemplates.Create(emailDto.MapToEmailModel());
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }


    public async Task<Result> Update(EditEmailDto emailDto)
    {
        var emailExistsResult = await _validationService.EmailTemplateExists(emailDto.Id);
        if (emailExistsResult.IsFailure)
        {
            return Result.Failure(EmailErrors.EmailTemplateNotFound);
        }

        var email = emailExistsResult.Value();

        email.From = emailDto.From;
        email.Subject = emailDto.Subject;
        email.Body = emailDto.Body;

        _unitOfWork.EmailTemplates.Update(email);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }


    public async Task<Result> Delete(Guid id)
    {
        var emailExistsResult = await _validationService.EmailTemplateExists(id);
        if (emailExistsResult.IsFailure)
        {
            return Result.Failure(EmailErrors.EmailTemplateNotFound);
        }

        var email = emailExistsResult.Value();

        Delete(email);
        return Result.Success();
    }

    public void Delete(EmailModel emailModel)
    {
        _unitOfWork.EmailTemplates.Delete(emailModel);
        _unitOfWork.SaveChangesAsync();
    }
}
