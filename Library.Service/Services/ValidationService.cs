using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Email;
using Library.Service.Dtos;
using Library.Service.Extensions;
using Library.Service.Interfaces;

namespace Library.Service.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool BirthdayIsValid(int year, int month, int day)
        {
            if (year < 1850 || year > DateTime.Today.Year) // validate year
                return false;

            if (month < 1 || month > 12) // validate month
                return false;

            if (day < 1 || day > DateTime.DaysInMonth(year, month)) // validate day (including leap years)
                return false;

            return true; // otherwise valid
        }

        public async Task<Result<Employee>> EmployeeExists(string id)
        {
            var employee = await _unitOfWork.Employees.GetById(new Guid(id));

            var employeeExists = employee is not null;

            if (!employeeExists)
                return Result.Failure<Employee>(EmployeeErrors.EmployeeNotFound);

            return Result.Success(employee!);
        }

        public async Task<Result<EmailModel>> EmailTemplateExists(string subject)
        {
            var email = await _unitOfWork.EmailTemplates.GetBySubject(subject);

            var emailExists = email is not null;

            if (!emailExists)
                return Result.Failure<EmailModel>(EmailErrors.EmailTemplateNotFound);

            return Result.Success(email!);
        }

        public async Task<Result<EmailModel>> EmailTemplateExists(Guid id)
        {
            var email = await _unitOfWork.EmailTemplates.GetById(id);

            var emailExists = email is not null;

            if (!emailExists)
                return Result.Failure<EmailModel>(EmailErrors.EmailTemplateNotFound);

            return Result.Success(email!);
        }

        public async Task<Result> EmailTemplateIsNew(string subject)
        {
            var email = await _unitOfWork.EmailTemplates.GetBySubject(subject);

            var emailExists = email is not null;

            if (emailExists)
                return EmailErrors.EmailTemplateAlreadyExists;

            return Result.Success();
        }
    }
}
