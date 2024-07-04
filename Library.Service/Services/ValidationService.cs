﻿using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Email;
using Library.Service.Interfaces;

namespace Library.Service.Services
{
    /// <inheritdoc />
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

        public async Task<Result<Employee>> EmployeeExists(string id)  // use when need to verify that employee exists
        {
            var employee = await _unitOfWork.Employees.GetById(new Guid(id), trackChanges: false);

            var employeeExists = employee is not null;

            if (!employeeExists)
                return Result.Failure<Employee>(EmployeeErrors.EmployeeNotFound);

            _unitOfWork.Detach(employee!);
            return employee; // implicit operator, wraps into Result.Success object
        }

        public async Task<Result<EmailModel>> EmailTemplateExists(string subject) // use when need to verify that email template exists
        {
            var email = await _unitOfWork.EmailTemplates.GetBySubject(subject);

            var emailExists = email is not null;

            if (!emailExists)
                return Result.Failure<EmailModel>(EmailErrors.EmailTemplateNotFound);

            return email; // implicit operator, wraps into Result.Success object
        }

        public async Task<Result<EmailModel>> EmailTemplateExists(Guid id) // use when need to verify that email template exists
        {
            var email = await _unitOfWork.EmailTemplates.GetById(id);

            if (email is null)
                return Result.Failure<EmailModel>(EmailErrors.EmailTemplateNotFound);

            return email;
        }

        public async Task<Result> EmailTemplateIsNew(string subject) // use when need to verify that email template does not exist
        {
            var email = await _unitOfWork.EmailTemplates.GetBySubject(subject);

            var emailExists = email is not null;

            if (emailExists)
                return EmailErrors.EmailTemplateAlreadyExists;

            return Result.Success();
        }

        public async Task<Result<Publisher>> PublisherExists(Guid id) // use when need to verify that publisher exists
        {
            var publisher = await _unitOfWork.Publishers.GetById(id);

            if (publisher is null)
            {
                return Result.Failure<Publisher>(PublisherErrors.PublisherNotFound);
            }

            return publisher; // implicit operator, wraps into Result.Success object
        }

        public async Task<Result> PublisherIsNew(string? email, string name) // use when need to verify that publisher does not exist
        {
            var publisher = email is null ?
                await _unitOfWork.Publishers.GetByName(name) :
                await _unitOfWork.Publishers.PublisherExists(email, name);

            if (publisher is not null)
            {
                return Result.Failure(PublisherErrors.PublisherAlreadyExists);
            }

            return Result.Success();
        }

        public async Task<Result<Author>> AuthorExists(Guid id) // use when need to verify that author exists
        {
            var author = await _unitOfWork.Authors.GetById(id);

            if (author is null)
            {
                return Result.Failure<Author>(AuthorErrors.AuthorNotFound);
            }

            return author; // implicit operator, wraps into Result.Success object
        }

        public async Task<Result> AuthorIsNew(string? email, string name) // use when need to verify that author does not exist
        {
            var author = email is null ?
                await _unitOfWork.Authors.GetByName(name) :
                await _unitOfWork.Authors.AuthorExists(email, name);

            if (author is not null)
            {
                return Result.Failure(AuthorErrors.AuthorAlreadyExists);
            }

            return Result.Success();
        }

        /// <summary>
        /// Checks if a book exists
        /// If yes, returns a Success result with Book. If no, returns Failure result with a corresponding error
        /// </summary>
        public async Task<Result<Book>> BookExists(Guid id)
        {
            var book = await _unitOfWork.Books.GetById(id);

            if (book is null)
            {
                return Result.Failure<Book>(BookErrors.BookNotFound);
            }

            return book;
        }

        public async Task<Result<Book>> BookExists(string isbn)
        {
            var book = await _unitOfWork.Books.GetOneWhere(x => x.ISBN == isbn);

            if (book is null)
            {
                return Result.Failure<Book>(BookErrors.BookNotFound);
            }

            return book;
        }

        public async Task<Result> BookIsNew(string isbn) // use when need to verify that book is new (when creating)
        {
            var book = await _unitOfWork.Books.GetOneWhere(x => x.ISBN == isbn);
            
            if (book is null)
            {
                return Result.Success();
            }

            return Result.Failure<Book>(BookErrors.BookAlreadyExists);
        }
    }
}
