using FluentValidation;
using Library.ViewModels.Reservations;

namespace Library.Validators.Reservation;

public class CreateReservationValidator : AbstractValidator<CreateReservationViewModel>
{
    public CreateReservationValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .NotNull()
            .MinimumLength(8).WithMessage("Id must be at most 9 characters long")
            .MaximumLength(11).WithMessage("Id must be at most 11 characters long")
            .Matches("^\\d+$").WithMessage("Invalid ID format");

        RuleFor(x => x.Books)
            .NotEmpty().WithMessage("Books field must not be empty");
    }
}
