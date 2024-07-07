using FluentValidation;
using Library.ViewModels.Customers;
using System.Text.RegularExpressions;

namespace Library.Validators.Customer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerViewModel>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .MinimumLength(8).WithMessage("Id must be at most 9 characters long")
            .MaximumLength(11).WithMessage("Id must be at most 11 characters long")
            .Matches("^\\d+$").WithMessage("Invalid ID format");

        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull().WithMessage("Name is required.");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .NotNull().WithMessage("Surname is required.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .NotNull().WithMessage("Email Address is required.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull().WithMessage("Address is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .NotNull().WithMessage("Phone Number is required.")
            .MinimumLength(7).WithMessage("PhoneNumber must not be less than 7 characters.")
            .MaximumLength(20).WithMessage("PhoneNumber must not exceed 20 characters.")
            .Matches(new Regex(@"^\d+$")).WithMessage("PhoneNumber not valid");

    }
}
