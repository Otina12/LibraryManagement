namespace Library.Service.Dtos.Report;

public record BooksDamagedReportDto(
    string ModelName,
    DateTime StartDate,
    DateTime EndDate
    );

