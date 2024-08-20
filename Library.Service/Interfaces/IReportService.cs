using Library.Model.Models.Report;
using Library.Service.Dtos.Report;
using Microsoft.AspNetCore.Mvc;

namespace Library.Service.Interfaces;

public interface IReportService
{
    Task<IEnumerable<PopularityReportRow>> GetPopularityReport(PopularityReportDto popularityReportDto);
    Task<IEnumerable<AnnualReportRow>> GetAnnualReport(AnnualReportDto annualReportDto);
}
