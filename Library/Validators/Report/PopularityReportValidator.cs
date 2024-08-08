using FluentValidation;
using Library.ViewModels.Reports;

namespace Library.Validators.Report;

public class PopularityReportValidator : AbstractValidator<PopularityReportViewModel>
{
    private static readonly HashSet<string> Models = new HashSet<string>()
    {
        "Author", "Publisher", "Book", "OriginalBook", "Genre", "Customer"
    };

    public PopularityReportValidator()
    {
        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage("Model name cannot be empty")
            .Must(x => Models.Contains(x)).WithMessage("Please choose a valid Model name");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate);
    }
}
