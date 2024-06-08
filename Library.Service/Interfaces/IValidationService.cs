using Library.Model.Abstractions;
using Library.Model.Models;
using Library.Model.Models.Email;

namespace Library.Service.Interfaces;

//general validation service for all models

/// <summary>
/// Provides methods to validate various models and return Success result with object or Failure result with corresponding error.
/// </summary>
public interface IValidationService
{
    bool BirthdayIsValid(int day, int month, int year);

    Task<Result<Employee>> EmployeeExists(string id);

    Task<Result<EmailModel>> EmailTemplateExists(string subject);
    Task<Result<EmailModel>> EmailTemplateExists(Guid id);
    Task<Result> EmailTemplateIsNew(string subject);

    Task<Result<Publisher>> PublisherExists(Guid id);
    Task<Result> PublisherIsNew(string? email, string name);

    Task<Result<Author>> AuthorExists(Guid id);
    Task<Result> AuthorIsNew(string? email, string name);

    Task<Result<Book>> BookExists(Guid id);
}
