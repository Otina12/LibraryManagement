using Library.Model.Models.Report;

namespace Library.Service.Dtos.Report;

public record BooksDamagedReportViewDto(
    string ModelName,
    IEnumerable<BooksDamagedReportRow> Items,
    DateTime StartDate,
    DateTime EndDate
    );
