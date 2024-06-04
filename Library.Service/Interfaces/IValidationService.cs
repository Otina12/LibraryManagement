using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Model.Models.Email;

namespace Library.Service.Interfaces;

//general validation service for all models
public interface IValidationService
{
    bool BirthdayIsValid(int day, int month, int year);
    Task<Result<Employee>> EmployeeExists(string id);
    Task<Result<EmailModel>> EmailTemplateExists(string subject);
    Task<Result<EmailModel>> EmailTemplateExists(Guid id);
    Task<Result> EmailTemplateIsNew(string subject);
}
