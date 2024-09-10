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

    Task<Result<Employee>> EmployeeExists(string Id, bool trackChanges = false);

    Task<Result<EmailModel>> EmailTemplateExists(string subject, bool trackChanges = false);
    Task<Result<EmailModel>> EmailTemplateExists(Guid Id, bool trackChanges = false);
    Task<Result> EmailTemplateIsNew(string subject, bool trackChanges = false);

    Task<Result<Publisher>> PublisherExists(Guid Id, bool trackChanges = false);
    Task<Result> PublisherIsNew(string? email, string name);

    Task<Result<Author>> AuthorExists(Guid id, bool trackChanges = false);
    Task<Result> AuthorIsNew(string? email, string name);

    Task<Result<Book>> BookExists(Guid Id, bool trackChanges = false);
    Task<Result<Book>> BookExists(string isbn, bool trackChanges = false);
    Task<Result> BookIsNew(string isbn, bool trackChanges = false);

    Task<Result<Customer>> CustomerExists(string Id, bool trackChanges = false);
    Task<Result> CustomerIsNew(string Id, bool trackChanges = false);

    Task<Result<Reservation>> ReservationExists(Guid Id, bool trackChanges = false);

    Task<Result<OriginalBook>> OriginalBookExists(Guid Id, bool trackChanges = false);
    Task<Result> OriginalBookIsNew(string title, int publishYear, bool trackChanges = false);

}
