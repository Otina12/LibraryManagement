using Library.Model.Models.Report;
using Library.Service.Dtos.Report;

namespace Library.Service.Interfaces;

public interface IReportService
{
    Task<IEnumerable<PopularityReportRow>> GetPopularityReport(PopularityReportDto popularityReportDto);
}
