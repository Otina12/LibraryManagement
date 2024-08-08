namespace Library.Service.Dtos.Report;

public record PopularityReportDto(
    string ModelName,
    DateTime StartDate,
    DateTime EndDate
    );
