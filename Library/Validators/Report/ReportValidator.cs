using FluentValidation;
using Library.Model.Enums;
using Library.ViewModels.Reports;

namespace Library.Validators.Report;

public class ReportValidator : AbstractValidator<ReportOptionsViewModel>
{
    private static readonly HashSet<string> Models = new HashSet<string>()
    {
        "Author", "Publisher", "Book", "OriginalBook", "Genre", "Customer", "Employee"
    };

    public ReportValidator()
    {
        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage("Model name cannot be empty")
            .Must(x => Models.Contains(x))
            .WithMessage("Please choose a valid Model name");

        When(x => x.ReportType == ReportType.Popularity, () =>
        {
            RuleFor(x => x.StartDate)
                .NotNull().WithMessage("Start date cannot be null for popularity reports");

            RuleFor(x => x.EndDate)
                .NotNull().WithMessage("End date cannot be null for popularity reports")
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be greater than or equal to Start date for popularity reports");
        });

        When(x => x.ReportType == ReportType.Annual, () =>
        {
            RuleFor(x => x.Year)
                .NotNull().WithMessage("Year cannot be null for annual reports");
        });
    }
}
