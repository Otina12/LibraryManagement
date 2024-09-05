using Library.Model.Models.Report;
using Library.Service.Dtos.Report;

namespace Library.Service.Interfaces;

public interface IReportService
{
    Task<IEnumerable<PopularityReportRow>> GetPopularityReport(PopularityReportDto popularityReportDto);
    Task<IEnumerable<AnnualReportRow>> GetAnnualReport(AnnualReportDto annualReportDto);
    Task<IEnumerable<BooksDamagedReportRow>> GetBooksDamagedReport(BooksDamagedReportDto booksDamagedReportDto);
}
