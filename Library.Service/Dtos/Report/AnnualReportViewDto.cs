using Library.Model.Models.Report;

namespace Library.Service.Dtos.Report;

public record AnnualReportViewDto(
    string ModelName,
    IEnumerable<AnnualReportRow> Items,
    int Year
    );
