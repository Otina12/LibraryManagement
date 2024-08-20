using Library.Model.Models.Report;

namespace Library.Service.Dtos.Report;

public record PopularityReportViewDto(
    string ModelName,
    IEnumerable<PopularityReportRow> Items,
    DateTime StartDate,
    DateTime EndDate
    );
